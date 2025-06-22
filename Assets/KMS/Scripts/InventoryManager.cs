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
        SetInstance();
        _model.Init();
    }

    public void ToggleInventory()
    {
        if (IsinventoryOpened)
        {
            Inventory.SetActive(false);
            _controller.CancelHolding();
        }
        else
        {
            Inventory.SetActive(true);
            _renderer.RenderInventory();
        }

    }

    public bool AddItem(ItemBase item, int amount, int dur)
    {
        bool canAdd = Controller.AddItem(item, amount, dur);
        _renderer.RenderInventory();
        return canAdd;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) InventoryManager.Instance.ToggleInventory();
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddItem(_model.ItemList.ItemList[0], 2, 30);
        if (Input.GetKeyDown(KeyCode.Alpha2)) AddItem(_model.ItemList.ItemList[1], 2, 10);
        if (Input.GetKeyDown(KeyCode.Alpha3)) AddItem(_model.ItemList.ItemList[2], 3, -1);
        HoldSlot.transform.position = Input.mousePosition;
    }


}
