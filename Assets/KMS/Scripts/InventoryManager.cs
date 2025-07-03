
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private GameObject _inventory;
    public GameObject Inventory { get { return _inventory; }}
    [SerializeField] private GameObject _quickslot;
    public GameObject Quickslot { get { return _quickslot; } }
    [SerializeField] private GameObject _holdSlot;
    public GameObject HoldSlot { get { return _holdSlot; } }

    [SerializeField] public ItemDictionary _itemDictionary;

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

    private ItemConsumeManage _consume;
    public ItemConsumeManage Consume => Consume;

    public bool IsinventoryOpened => Inventory.activeSelf;

    protected override bool ShouldDontDestroy => false;


    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    private void Init()
    {
        _renderer = GetComponent<InventoryRenderer>();
        _controller = GetComponent<InventoryController>();
        _model = GetComponent<InventoryModel>();
        _craft = GetComponent<CraftingController>();
        _hand = GetComponent<PlayerHandItemController>();
        _consume = GetComponent<ItemConsumeManage>();
        _model.Init();
    }

    public void ToggleUI()
    {
        UIManager.Instance.ToggleUI(ModalUI.inventory);
        _renderer.RenderInventory();
    }

    public void AlignInvWithCraft()
    {
        UIManager.Instance.InvUI.transform.position = UIManager.Instance.CraftUI.transform.position;
        //Debug.Log(111);
    }
    public void AlignCraftWithInv()
    {
        UIManager.Instance.CraftUI.transform.position = UIManager.Instance.InvUI.transform.position;
        //Debug.Log(222);
    }

    public bool AddItem(ItemBase item, int amount, int dur)
    {
        bool canAdd = Controller.AddItem(item, amount, dur);
        _renderer.RenderInventory();
        return canAdd;
    }

    public int GetNullSpaceCount()
    {
        int sum = 0;
        for (int i = 6; i < _model.InvItems.Length; i++)
        {
            if (_model.InvItems[i] == null) sum++;
        }
        return sum;
    }


    public void DecreaseWeaponDurability(int amount = 1)
    {
        _controller.Dur(0, amount);
    }
    public void DecreaseShieldDurability(int amount = 1)
    {
        _controller.Dur(1, amount);
    }
    public bool AddItemByID(int id, int count)
    {
        int d = Craft.GetMaxDur(id);
        if( Craft.AddItemByID(id, count, d))
        {
            StartCoroutine(CheckQuestItem());
            return true;
        }
        return false;
    }

    public IEnumerator CheckQuestItem()
    {
        yield return new WaitForEndOfFrame();
        QuestData completed = null;
        foreach (QuestData q in QuestManager.Instance.AcceptedItemQuestList)
        {
            int.TryParse(q.RequiredItemID, out int reqID);
            if (_craft.CountByID[reqID] >= q.RequiredItemQuantity) //충분히 가지고 있음
            {
                QuestManager.Instance.CompleteQuest(q.QuestID);
                //QuestManager.Instance.AcceptedItemQuestList.Remove(q);
            }
        }
        if (completed != null) QuestManager.Instance.AcceptedItemQuestList.Remove(completed);
    }
    public bool RemoveItemByID(int id, int count)
    {
        return Craft.RemoveItemByID(id, count);
    }
    public void ReduceRightHandItem()
    {
        Controller.ReduceEquippedItem(0, 1);
    }
    public bool FindItemByID(int id, bool remove = true) //해당 아이템 있으면 1개 지우고 true 반환.
    {
        if (_craft.CountByID[id] > 0)
        {
            if (remove) // true(기본값) 이면 지움
            {
                _controller.RemoveItem(_itemDictionary.ItemDic[id], 1);
            }
            return true;
        }
        return false;
    }

    public ItemBase CurrentWeapon()
    {
        if (Controller.EquippedSlotIndex[0] == -1)
        {
            return null;
        }
        else
        {
            return _model.InvItems[Controller.EquippedSlotIndex[0]].Data;
        }
    }

    private List<int> _itemlist;
    int num = 0;
    private void Start()
    {
        _itemlist = new();
        foreach (int q in _craft._itemDictionary.ItemDic.Keys)
        {
            _itemlist.Add(q);
        }
    }
    private void Update()
    {
        if (!UISceneLoader.Instance.Playerattack.IsAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) Controller.Equip(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Controller.Equip(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Controller.Equip(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) Controller.Equip(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) Controller.Equip(4);
            if (Input.GetKeyDown(KeyCode.Alpha6)) Controller.Equip(5);
        }

        if (Input.GetKeyDown(KeyCode.P)) DecreaseWeaponDurability();
        if (Input.GetKeyDown(KeyCode.O)) DecreaseShieldDurability();
        

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            num++;
            if (num == _itemlist.Count) num = 0;
            Debug.Log(_itemlist[num]);
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            num--;
            if (num == -1) num = _itemlist.Count - 1;
            Debug.Log(_itemlist[num]);
        }
        if (Input.GetKeyDown(KeyCode.Home))
        {
            AddItemByID(_itemlist[num], 1);
        }
    }
    /// <summary>
    /// 0 <- 오른손 아이템, 1 <- 왼손 아이템
    /// 양손을 사용하는 아이템일 경우 반환값이 같습니다
    /// 해당 손에 아무것도 들려있지 않을 경우 null을 반환합니다.
    /// </summary>
    /// <param name="hand"></param>
    public ItemBase GetHandItem(int hand)
    {
       return _model.InvItems[_controller.EquippedSlotIndex[hand]].Data;
    }
}
