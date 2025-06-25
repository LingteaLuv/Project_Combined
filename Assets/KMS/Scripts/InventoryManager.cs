using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : SingletonT<InventoryManager>
{
    [SerializeField] private GameObject _inventory;
    public GameObject Inventory { get { return _inventory; }}
    [SerializeField] private GameObject _quickslot;
    public GameObject Quickslot { get { return _quickslot; } }
    [SerializeField] private GameObject _holdSlot;
    public GameObject HoldSlot { get { return _holdSlot; } }

    private InventoryModel _model;
    public InventoryModel Model => _model;

    private InventoryRenderer _renderer;
    public InventoryRenderer Renderer => _renderer;

    private InventoryController _controller;
    public InventoryController Controller => _controller;

    private CraftingController _craft;
    public CraftingController Craft => _craft;

    private PlayerHandItemController _hand;
    public PlayerHandItemController Hand => _hand;

    public bool IsinventoryOpened => Inventory.activeSelf;


    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _renderer = GetComponent<InventoryRenderer>();
        _controller = GetComponent<InventoryController>();
        _model = GetComponent<InventoryModel>();
        _craft = GetComponent<CraftingController>();
        _hand = GetComponent<PlayerHandItemController>();
        SetInstance();
        _model.Init();
    }

    public void ToggleUI()
    {
        UIManage.Instance.ToggleUI(ModalUI.inventory);
        _renderer.RenderInventory();
    }

    public void AlignInvWithCraft()
    {
        UIManage.Instance.InvUI.transform.position = UIManage.Instance.CraftUI.transform.position;
        //Debug.Log(111);
    }
    public void AlignCraftWithInv()
    {
        UIManage.Instance.CraftUI.transform.position = UIManage.Instance.InvUI.transform.position;
        //Debug.Log(222);
    }

    public bool AddItem(ItemBase item, int amount, int dur)
    {
        bool canAdd = Controller.AddItem(item, amount, dur);
        _renderer.RenderInventory();
        return canAdd;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) ToggleUI();



        if (Input.GetKeyDown(KeyCode.Z)) AddItem(_model.ItemList.ItemList[0], 3, -1);
        if (Input.GetKeyDown(KeyCode.X)) AddItem(_model.ItemList.ItemList[1], 1, 50);
        if (Input.GetKeyDown(KeyCode.C)) AddItem(_model.ItemList.ItemList[2], 1, 25);
        if (Input.GetKeyDown(KeyCode.V)) AddItem(_model.ItemList.ItemList[3], 1, 100);
        if (Input.GetKeyDown(KeyCode.B)) AddItem(_model.ItemList.ItemList[4], 1, -1);
        if (Input.GetKeyDown(KeyCode.N)) AddItem(_model.ItemList.ItemList[5], 1, -1);


        if (Input.GetKeyDown(KeyCode.Alpha1)) Controller.Equip(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Controller.Equip(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Controller.Equip(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Controller.Equip(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Controller.Equip(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) Controller.Equip(5);


        HoldSlot.transform.position = Input.mousePosition;
    }

    public bool FindItem(int ID, bool remove)
    {
        return false;
    }
}
