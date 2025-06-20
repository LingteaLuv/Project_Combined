using System.Collections;
using System.Collections.Generic;
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

    private InvSlotController[] _inventorySlots;
    public InvSlotController[] InventorySlots { get { return _inventorySlots; } }

    private Image[] _invSlotItemImages;
    public Image[] InvSlotItemImages { get { return _invSlotItemImages; } }

    private TMP_Text[] _invSlotItemAmountTexts;
    public TMP_Text[] InvSlotItemAmountTexts { get { return _invSlotItemAmountTexts; } }

    private Image _holdSlotItemImage;
    public Image HoldSlotItemImage { get { return _holdSlotItemImage; } }

    private TMP_Text _holdSlotItemAmountText;
    public TMP_Text HoldSlotItemAmountText { get { return _holdSlotItemAmountText; } }

    private ItemSO[] _invItems;
    public ItemSO[] InvItems { get { return _invItems; } }

    private int[] _invItemAmounts;
    public int[] InvItemAmounts { get { return _invItemAmounts; } }

    private ItemSO _heldItem;
    public ItemSO HeldItem { get { return _heldItem; } set { _heldItem = value; } }

    private int _heldItemAmount;
    public int HeldItemAmount { get { return _heldItemAmount; } set { _heldItemAmount = value; } }

    public int slotCount => _inventorySlots.Length;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _inventorySlots = _inventory.GetComponentsInChildren<InvSlotController>();
        _invSlotItemImages = new Image[slotCount];
        _invSlotItemAmountTexts = new TMP_Text[slotCount];
        _invItems = new ItemSO[slotCount];
        _invItemAmounts = new int[slotCount];

        for (int i = 0; i < slotCount; i++)
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
