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

    private RectTransform _lootRect;
    private RectTransform _invRect;
    private void Awake()
    {
        _lootRect = LootUI.GetComponent<RectTransform>();
        _invRect = InvUI.GetComponent<RectTransform>();
        SetInstance();
        IsModalUIOpened = false;
        Current = ModalUI.nothing;
    }

    public bool IsModalUIOpened { get; set; }
    public ModalUI Current { get; set; }
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

    }
    public void CloseUI()
    {
        if (!IsModalUIOpened) return;
        IsModalUIOpened = false;
        Current = ModalUI.nothing;
        LootUI.SetActive(false);
        InvUI.SetActive(false);
        ModalBase.SetActive(false);
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
}
