using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ModalUI
{
    nothing,
    inventory,
    lootTable
}
public class UIManage : SingletonT<UIManage>
{
    [SerializeField] public GameObject ModalBase;
    [SerializeField] public GameObject LootUI;
    [SerializeField] public GameObject InvUI;
    [SerializeField] public GameObject CraftUI;

    [SerializeField] public CanvasGroup UIGroup;

    private RectTransform _lootRect;
    private RectTransform _invRect;

    private Coroutine _coroutine;
    private WaitForEndOfFrame _wait;

    public bool IsModalUIOpened { get; set; }
    public ModalUI Current { get; set; }
    private void Awake()
    {
        _lootRect = LootUI.GetComponent<RectTransform>();
        _invRect = InvUI.GetComponent<RectTransform>();
        SetInstance();
        IsModalUIOpened = false;
        Current = ModalUI.nothing;
        UIGroup.alpha = 0;
        _wait = new WaitForEndOfFrame();
    }

    private void Start()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseUI();
    }
    public void OpenUI(ModalUI cur)
    {
        if (IsModalUIOpened) return;
        IsModalUIOpened = true;
        ModalBase.SetActive(true);
        Current = cur;
        if (Current == ModalUI.inventory)
        {
            InvUI.SetActive(true);
            SetUIPos(_invRect, 750, 450);
        }
        else if (Current == ModalUI.lootTable)
        {
            LootUI.SetActive(true);
            SetUIPos(_lootRect, 500, 600);
            InvUI.SetActive(true);
            SetUIPos(_invRect, 1100, 450);
        }

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);

        }
        _coroutine = StartCoroutine(FadeIn());

    }
    public void CloseUI()
    {
        if (!IsModalUIOpened) return;
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
        UITrs.position = new Vector3(x, y, 0); 
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
        ModalBase.SetActive(false);
        IsModalUIOpened = false;
        Current = ModalUI.nothing;
    }
}
