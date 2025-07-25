using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TextManager : Singleton<TextManager>
{
    // 팝업 문구를 출력하기 위한 UI
    private PopUpUI _popUpUI;
    
    // 메모 팝업 문구를 출력하기 위한 UI
    private MemoPopupUI _memoPopUpUI;
    
    // CSV파일의 데이터를 가지고 있는 TextLoader를 참조
    [FormerlySerializedAs("_dialogDic")] [SerializeField] private PopUpDictionary _popUpDic;

    private bool _isOnPopUp;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    
    // 초기화 순서를 위해 외부 참조(PopUp UI)의 경우 Start()에서 처리
    private void Start()
    {
        ConfigUI();
        _popUpDic.Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HideText();
        }
    }

    private void Init()
    {
        _isOnPopUp = false;
    }
    
    // PopUp UI 초기화, UI Binder를 활용
    private void ConfigUI()
    {
        if (_popUpUI == null)
        {
            _popUpUI = UIBinder.Instance.GetPopupUI();
        }
        if (_memoPopUpUI == null)
        {
            _memoPopUpUI = UIBinder.Instance.GetMemoPopUpUI();
        }
    }
    
    // CSV 파일에 저장된 text를 팝업창에 띄우는 메서드
    private void PopUpText(string id)
    {
        // 외부에서 해당 메서드가 호출되어 TextManager가 생성되는 경우 
        // Start보다 먼저 수행되기 때문에 ConfigUI를 호출해야한다.   
        // 첫 씬에서 Manager 빈 오브젝트에 묶어 생성할 경우 호출하지 않아도 무관
        ConfigUI();
        
        // CSV 파일에 저장되어있는 데이터 불러오기
        string[] popupTexts = _popUpDic.GetPopUpText(id);
        //string popupHeadText = _dialogDic.GetPopupText(id);
        // PopUp UI 활성화 및 출력
        _popUpUI.gameObject.SetActive(true);
        _popUpUI.PupUpHeadText(popupTexts[0]);
        _popUpUI.PopUpText(popupTexts[1]);
    }
    
    public void MemoPopUpText(string id)
    {
        // 외부에서 해당 메서드가 호출되어 TextManager가 생성되는 경우 
        // Start보다 먼저 수행되기 때문에 ConfigUI를 호출해야한다.   
        // 첫 씬에서 Manager 빈 오브젝트에 묶어 생성할 경우 호출하지 않아도 무관
        ConfigUI();
        
        // CSV 파일에 저장되어있는 데이터 불러오기
        string[] popupTexts = _popUpDic.GetPopUpText(id);
        //string popupHeadText = _dialogDic.GetPopupText(id);
        Debug.Log(_memoPopUpUI);
        Debug.Log(popupTexts[0]);
        // PopUp UI 활성화 및 출력
        _memoPopUpUI.gameObject.SetActive(true);
        _memoPopUpUI.PupUpHeadText(popupTexts[0]);
        _memoPopUpUI.PopUpText(popupTexts[1]);
    }
    
    // PopUp UI를 닫는 메서드
    private void HideText()
    {
        // 문구 초기화 및 UI 비활성화
        if (_popUpUI != null && _popUpUI.gameObject.activeSelf)
        {
            _popUpUI.ResetText();
            _popUpUI.gameObject.SetActive(false);
        }
        if (_memoPopUpUI != null && _memoPopUpUI.gameObject.activeSelf)
        {
            _memoPopUpUI.ResetText();
            _memoPopUpUI.gameObject.SetActive(false);
        }
    }

    // 일정 시간(time)이 지나면 출력된 PopUp UI창이 닫히도록 구현한 Coroutine
    private IEnumerator PopupTextRoutine(string id, float time)
    {
        _isOnPopUp = true;
        PopUpText(id);
        yield return new WaitForSeconds(time);
        HideText();
        _isOnPopUp = false;
    }
    
    // 외부에서 호출하여 실제로 사용되는 메서드
    public void PopupTextForSecond(string id, float time)
    {
        if (!_isOnPopUp)
        {
            StartCoroutine(PopupTextRoutine(id, time));
        }
    }
}
