using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManage : SingletonT<UIManage>
{
    [SerializeField] public GameObject ModalBase;
    [SerializeField] public GameObject LootUI;
    [SerializeField] public GameObject InvUI;

    public enum ModalUI
    {
        inventory,
        lootTable
    }
    private void Awake()
    {
        SetInstance();
        IsModalUIOpened = false;
    }

    public bool IsModalUIOpened { get; set; }
    public ModalUI Current { get; set; }
    public void OpenUI()
    {
        if (IsModalUIOpened) return;
        IsModalUIOpened = true;
        ModalBase.SetActive(true);
        if (Current == ModalUI.inventory)
        {
            LootUI.SetActive(true);
            SetUIPos(LootUI.transform, 1000, 600);
        }
        else if (Current == ModalUI.lootTable)
        {
            LootUI.SetActive(true);
            SetUIPos(LootUI.transform, 500, 600);
            InvUI.SetActive(true);
            SetUIPos(InvUI.transform, 1300, 600);
        }

    }
    public void CloseModalUI()
    {
        if (!IsModalUIOpened) return;
        IsModalUIOpened = false;
        LootUI.SetActive(false);
        InvUI.SetActive(false);
        ModalBase.SetActive(false);
    }

    private void SetUIPos(Transform UITrs, int x, int y)
    {
        UITrs.position.Set(x, y, 0);
    }
}
