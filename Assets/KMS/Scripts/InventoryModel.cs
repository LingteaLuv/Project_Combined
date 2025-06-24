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

    [SerializeField] public TMP_Text Desc;
    [SerializeField] public GameObject TrashButton;
    [SerializeField] public GameObject UtilButton;
    [SerializeField] public TMP_Text UtilButtonText;

    public Item[] InvItems { get; private set; }

    public InvSlotController[] InventorySlots { get; private set; }
    public InvSlotController[] QuickSlotSlots { get; private set; }
    public Image[] InvSlotPanelImages { get; private set; }
    public Image[] InvSlotItemImages { get; private set; }
    public TMP_Text[] InvSlotItemAmountTexts { get; private set; }
    public Slider[] InvSlotItemDurSliders { get; private set; }

    public Image HoldSlotItemImage { get; set; }
    public TMP_Text HoldSlotItemAmountText { get; set; }

    public Color SlotColor { get; private set; }

    //public ItemSO HeldItem { get; set; }
    //public int HeldItemAmount { get; set; }
    //public int HeldItemDurability { get; set; }

    public int SlotCount => InventorySlots.Length + QuickSlotSlots.Length;

    public void Init()
    {
        InventorySlots = InventoryManager.Instance.Inventory.GetComponentsInChildren<InvSlotController>();
        QuickSlotSlots = InventoryManager.Instance.Quickslot.GetComponentsInChildren<InvSlotController>();

        InvSlotPanelImages = new Image[SlotCount];
        InvSlotItemImages = new Image[SlotCount];
        InvSlotItemAmountTexts = new TMP_Text[SlotCount];
        InvSlotItemDurSliders = new Slider[SlotCount];

        InvItems = new Item[SlotCount];
        int i = 0;
        for (i = 0; i < QuickSlotSlots.Length; i++)
        {
            InvSlotPanelImages[i] = QuickSlotSlots[i].gameObject.GetComponentsInChildren<Image>()[0];
            InvSlotItemImages[i] = QuickSlotSlots[i].gameObject.GetComponentsInChildren<Image>()[1];
            InvSlotItemAmountTexts[i] = QuickSlotSlots[i].gameObject.GetComponentInChildren<TMP_Text>();
            InvSlotItemDurSliders[i] = QuickSlotSlots[i].gameObject.GetComponentInChildren<Slider>();
        }
        for (i = i; i < SlotCount; i++)
        {
            InvSlotPanelImages[i] = InventorySlots[i - QuickSlotSlots.Length].gameObject.GetComponentsInChildren<Image>()[0];
            InvSlotItemImages[i] = InventorySlots[i - QuickSlotSlots.Length].gameObject.GetComponentsInChildren<Image>()[1];
            InvSlotItemAmountTexts[i] = InventorySlots[i - QuickSlotSlots.Length].gameObject.GetComponentInChildren<TMP_Text>();
            InvSlotItemDurSliders[i] = InventorySlots[i - QuickSlotSlots.Length].gameObject.GetComponentInChildren<Slider>();
        }

        //HeldItem = null;
        //HeldItemAmount = 0;
        //HeldItemDurability = 0;
        HoldSlotItemImage = InventoryManager.Instance.HoldSlot.GetComponentInChildren<Image>();
        HoldSlotItemAmountText = InventoryManager.Instance.HoldSlot.GetComponentInChildren<TMP_Text>();

        SlotColor = InvSlotPanelImages[0].color;
    }


}
