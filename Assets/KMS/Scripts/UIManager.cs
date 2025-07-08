using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public enum ModalUI
{
    nothing,
    inventory,
    lootTable,
    quest,
    map
}
public class UIManager : Singleton<UIManager>
{
    [SerializeField] public GameObject ModalBase;
    [SerializeField] public GameObject LootUI;
    [SerializeField] public GameObject InvUI;
    [SerializeField] public GameObject CraftUI;
    [SerializeField] public GameObject QuestLogUI;
    [SerializeField] public GameObject MapUI;

    [SerializeField] public GameObject MemoUI;


    [SerializeField] public GameObject QuestCont;

    [SerializeField] public CanvasGroup UIGroup;

    [SerializeField] private ParameterHudUI _propHUI;

    [SerializeField] private GameObject _quickslot;

    [SerializeField] private PauseUIControl _puc;

    private PlayerLooting _playerLoot;
    private PlayerNPCInteractor _playerNPCInt;
    private RectTransform _lootRect;
    private RectTransform _invRect;
    private RectTransform _questRect;
    private RectTransform _mapRect;

    private Coroutine _coroutine;
    private WaitForEndOfFrame _wait;

    private PlayerCameraController _pcc;
    private PlayerMovement _pm;

    public Property<bool> IsUIOpened;

    private bool _UILock;
    public ModalUI Current { get; set; }

    protected override bool ShouldDontDestroy => false;
    protected override void Awake()
    {

        base.Awake();
        _lootRect = LootUI.GetComponent<RectTransform>();
        _invRect = InvUI.GetComponent<RectTransform>();
        _questRect = QuestLogUI.GetComponent<RectTransform>();
        _mapRect = MapUI.GetComponent<RectTransform>();
        _playerLoot = UISceneLoader.Instance.Playerattack.gameObject.GetComponentInChildren<PlayerLooting>();
        _playerNPCInt = UISceneLoader.Instance.Playerattack.gameObject.GetComponentInChildren<PlayerNPCInteractor>();
        _pcc = UISceneLoader.Instance.Playerattack.GetComponent<PlayerCameraController>();
        _pm = UISceneLoader.Instance.Playerattack.GetComponent<PlayerMovement>();
        IsUIOpened = new Property<bool>(false);
        Current = ModalUI.nothing;
        UIGroup.alpha = 0;
        _wait = new WaitForEndOfFrame();

    }
    public void OffQuickslot()
    {
        _quickslot.SetActive(false);
    }
    public void OnQuickslot()
    {

            _quickslot.SetActive(true);

    }
    public void OffHUI()
    {
            _propHUI.gameObject.SetActive(false);

        
    }
    public void OnHUI()
    {
            _propHUI.gameObject.SetActive(true);

    }


    public void Lock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _pm.MoveLock();
        _pcc.PauseCamera();
        _puc.LockPause();
        LockUIUpdate();
    }
    public void UnLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _pm.MoveUnLock();
        _pcc.ResumeCamera();
        _puc.UnlockPasue();
        UnlockUIUpdate();
    }
    private void Start() //다꺼줌
    {
        ModalBase.SetActive(false);
        LootUI.SetActive(false);
        InvUI.SetActive(false);
        CraftUI.SetActive(false);
        QuestLogUI.SetActive(false);
        MapUI.SetActive(false);
        
        IsUIOpened.OnChanged += SetCursorLock;
        IsUIOpened.OnChanged += SetCameraLock;
        IsUIOpened.OnChanged += SetMoveLock;
        IsUIOpened.OnChanged += SetAttackLock;
       

        _playerNPCInt.OnInteract2 += _pcc.PauseCamera;
        _playerNPCInt.OnInteract2 += _pm.MoveLock;
        _playerNPCInt.OnInteract2 += LockUIUpdate;
        _playerNPCInt.OnInteract2 += CursorUnlock;
        _playerNPCInt.OnInteract2 += LockAttacking;
        _playerNPCInt.OnInteract2 += _puc.LockPause;

        DialogueManager.Instance.OffDialogue += _pcc.ResumeCamera;
        DialogueManager.Instance.OffDialogue += _pm.MoveUnLock;
        DialogueManager.Instance.OffDialogue += UnlockUIUpdate;
        DialogueManager.Instance.OffDialogue += CursorLock;
        DialogueManager.Instance.OffDialogue += UnlockAttacking;
        DialogueManager.Instance.OffDialogue += _puc.UnlockPasue;

        SetCursorLock(IsUIOpened.Value);



    }

    public void CameraLock()
    {
        _pcc.PauseCamera();
    }
    public void CameraUnlock()
    {
        _pcc.ResumeCamera();
    }
    public void MoveLock()
    {
        _pm.MoveLock();
    }
    public void MoveUnlock()
    {
        _pm.MoveUnLock();
    }

    public void CursorLock()
    {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
    }
    public void CursorUnlock()
    {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
    }
    private void SetMoveLock(bool isUIOpened)
    {
        if (!isUIOpened)
        {
            _pm.MoveUnLock();
        }
        else
        {
            _pm.MoveLock();
        }
    }
    private void SetCameraLock(bool isUIOpened) {
        if (!isUIOpened)
        {
            _pcc.ResumeCamera();
        }
        else
        {
            _pcc.PauseCamera();
        }
    }
    private void SetAttackLock(bool isUIOpened)
    {
        if (!isUIOpened)
        {
            UISceneLoader.Instance.Playerattack.IsAttacking = false;
        }
        else
        {
            UISceneLoader.Instance.Playerattack.IsAttacking = true;
        }
    }
    private void SetCursorLock(bool isUIOpened)
    {
        if (!isUIOpened)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void LockAttacking()
    {
        UISceneLoader.Instance.Playerattack.IsAttacking = true;
    }
    public void UnlockAttacking()
    {
        UISceneLoader.Instance.Playerattack.IsAttacking = false;
    }
   

    public void LockUIUpdate()
    {
        _UILock = true;
        CloseUI();
    }
    public void UnlockUIUpdate()
    {
        _UILock = false;
    }
    
    private void Update()
    {
        if (_UILock) return;
        if (Input.GetKeyDown(KeyCode.Escape)) CloseUI();
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleUI(ModalUI.inventory);
            InventoryManager.Instance.Renderer.RenderInventory();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_playerNPCInt.TryInteract())
            {
                if (Current == ModalUI.inventory || Current == ModalUI.quest) return;
                _playerLoot.TryLoot();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleUI(ModalUI.quest);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleUI(ModalUI.map);
        }
    }
    public void OpenUI(ModalUI cur)
    {
        if (IsUIOpened.Value) return; //다른게 열려있음
        if (UISceneLoader.Instance.Playerattack.IsAttacking) return; // 공격 중에 불가
        AudioManager.Instance.PlayUISFX(AudioManager.Instance._sfxDic["Bag 15"]);
        IsUIOpened.Value = true;
        ModalBase.SetActive(true);
        Current = cur;
        if (Current == ModalUI.inventory)
        {
            InvUI.SetActive(true);
            SetUIPos(_invRect, 0, -100);
        }
        else if (Current == ModalUI.lootTable)
        {
            LootUI.SetActive(true);
            SetUIPos(_lootRect, -200, 0);
            InvUI.SetActive(true);
            SetUIPos(_invRect, 350, -100);
        }
        else if (Current == ModalUI.quest)
        {
            QuestLogUI.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(QuestCont.transform as RectTransform);
            SetUIPos(_questRect, 0, 0);
        }
        else if (Current == ModalUI.map)
        {
            MapUI.SetActive(true);
            SetUIPos(_mapRect, 0, 0);
        }

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);

        }
        _coroutine = StartCoroutine(FadeIn());

    }
    public void CloseUI()
    {
        if (!IsUIOpened.Value) return; //열린게 없음
        if (MemoUI.activeSelf) return;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);

        }
        _coroutine = StartCoroutine(FadeOut());
    }
    public void ToggleUI(ModalUI cur)
    {
        if (Current == cur)
        {
            CloseUI();
        }
        else
        {
            OpenUI(cur);
        }
    }

    private void SetUIPos(RectTransform UITrs, int x, int y)
    {
        //UITrs.position = new Vector3(x, y, 0); 
        UITrs.localPosition = new Vector3(x, y, 0);
    }
    private IEnumerator FadeIn()
    {
        while (UIGroup.alpha < 1)
        {
            UIGroup.alpha += 0.05f;
            yield return _wait;
        }
        UIGroup.alpha = 1f;
    }
    private IEnumerator FadeOut()
    {
        while (UIGroup.alpha > 0)
        {
            UIGroup.alpha -= 0.05f;
            yield return _wait;
        }
        UIGroup.alpha = 0f;
        LootUI.SetActive(false);
        InvUI.SetActive(false);
        CraftUI.SetActive(false);
        QuestLogUI.SetActive(false);
        ModalBase.SetActive(false);
        MapUI.SetActive(false);
        IsUIOpened.Value = false;
        Current = ModalUI.nothing;
    }
}
