using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryModel : MonoBehaviour
{
    [SerializeField] private ItemListSO _itemList;
    public ItemListSO ItemList { get { return _itemList; } }

    [SerializeField] private GameObject _inventory;
    public GameObject Inventory { get { return _inventory; } }

    [SerializeField] private GameObject _holdSlot;
    public GameObject HoldSlot { get { return _holdSlot; } }

    //[SerializeField] private GameObject _invSlotPanel;
    //[SerializeField] private GameObject _weaponSlotPanel;
    //
    //[SerializeField] private int _invSlotCount;
    //[SerializeField] private int _weaponSlotCount;
    //
    //[SerializeField] private GameObject _slot; 

    private InvSlotController[] _inventorySlots;
    public InvSlotController[] InventorySlots { get { return _inventorySlots; } }

    private Image[] _invSlotItemImages;
    public Image[] InvSlotItemImages { get { return _invSlotItemImages; } }

    private TMP_Text[] _invSlotItemAmountTexts;
    public TMP_Text[] InvSlotItemAmountTexts { get { return _invSlotItemAmountTexts; } }

    private Image _holdSlotItemImage;
    public Image HoldSlotItemImage { get { return _holdSlotItemImage; } set { _holdSlotItemImage = value; } }

    private TMP_Text _holdSlotItemAmountText;
    public TMP_Text HoldSlotItemAmountText { get { return _holdSlotItemAmountText; } set { _holdSlotItemAmountText = value; } }

    private ItemSO[] _invItems;
    public ItemSO[] InvItems { get { return _invItems; } }

    private int[] _invItemAmounts;
    public int[] InvItemAmounts { get { return _invItemAmounts; } }

    private ItemSO _heldItem;
    public ItemSO HeldItem { get { return _heldItem; } set { _heldItem = value; } }

    private int _heldItemAmount;
    public int HeldItemAmount { get { return _heldItemAmount; } set { _heldItemAmount = value; } }

    public int slotCount => _inventorySlots.Length;

    public int InvSlotIndexBound = 13;
    public int WeaponSlotIndexBound = 17;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        //int ie;
        //for (ie = 0; ie < _invSlotCount; ie++)
        //{
        //    GameObject s = Instantiate(_slot, _invSlotPanel.transform);
        //    s.name = ie.ToString();
        //}
        //for (ie = ie; ie < _weaponSlotCount; ie++)
        //{
        //    GameObject s = Instantiate(_slot, _weaponSlotPanel.transform);
        //    s.name = ie.ToString();
        //}
        _inventorySlots = _inventory.GetComponentsInChildren<InvSlotController>();
        _invSlotItemImages = new Image[slotCount];
        _invSlotItemAmountTexts = new TMP_Text[slotCount];
        _invItems = new ItemSO[slotCount];
        _invItemAmounts = new int[slotCount];
        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            _invSlotItemImages[i] = _inventorySlots[i].gameObject.GetComponentsInChildren<Image>()[1];
            _invSlotItemAmountTexts[i] = _inventorySlots[i].gameObject.GetComponentInChildren<TMP_Text>();
        }
        _heldItem = null;
        _heldItemAmount = 0;
        _holdSlotItemImage = _holdSlot.GetComponentInChildren<Image>();
        _holdSlotItemAmountText = _holdSlot.GetComponentInChildren<TMP_Text>();
    }


}
