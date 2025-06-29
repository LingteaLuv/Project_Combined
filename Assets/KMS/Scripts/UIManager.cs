using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;


public enum ModalUI
{
    nothing,
    inventory,
    lootTable
}
public class UIManager : SingletonT<UIManager>
{
    [SerializeField] public GameObject ModalBase;
    [SerializeField] public GameObject LootUI;
    [SerializeField] public GameObject InvUI;
    [SerializeField] public GameObject CraftUI;

    [SerializeField] public CanvasGroup UIGroup;

    private PlayerLooting _playerLoot;
    private RectTransform _lootRect;
    private RectTransform _invRect;

    private Coroutine _coroutine;
    private WaitForEndOfFrame _wait;

    public Property<bool> IsUIOpened;
    public ModalUI Current { get; set; }
    private void Awake()
    {
        _lootRect = LootUI.GetComponent<RectTransform>();
        _invRect = InvUI.GetComponent<RectTransform>();
        _playerLoot = UISceneLoader.Instance.Playerattack.gameObject.GetComponentInChildren<PlayerLooting>();
        SetInstance();
        IsUIOpened = new Property<bool>(false);
        Current = ModalUI.nothing;
        UIGroup.alpha = 0;
        _wait = new WaitForEndOfFrame();
    }

    private void Start() //다꺼줌
    {
        ModalBase.SetActive(false);
        LootUI.SetActive(false);
        InvUI.SetActive(false);
        CraftUI.SetActive(false);
        
        IsUIOpened.OnChanged += SetCursorLock;
        SetCursorLock(IsUIOpened.Value);
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
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseUI();
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleUI(ModalUI.inventory);
            InventoryManager.Instance.Renderer.RenderInventory();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Current == ModalUI.inventory) return;
            _playerLoot.TryLoot();
        }
    }
    public void OpenUI(ModalUI cur)
    {
        if (IsUIOpened.Value) return; //다른게 열려있음
        IsUIOpened.Value = true;
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
        if (!IsUIOpened.Value) return; //열린게 없음
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
        IsUIOpened.Value = false;
        Current = ModalUI.nothing;
    }
}
