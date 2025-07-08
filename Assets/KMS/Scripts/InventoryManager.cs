
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public ItemConsumeManage Consume => _consume;

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


        if (Controller.AddItem(item, amount, dur))
        {
            StartCoroutine(CheckQuestItem());
            return true;
        }
        return false;
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
        if (count == 0) return true;
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
            if (q.Type == QuestType.Delivery) // 배달 퀘스트에 한해 요구하는 수량과 동일하게 가지고 있어야 함
            {
                if (_craft.CountByID[reqID] == q.RequiredItemQuantity)
                {
                    QuestManager.Instance.CompleteQuest(q.QuestID);
                }
                continue;
            }
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
        if (count == 0) return true;
        if (Craft.RemoveItemByID(id, count))
        {
            StartCoroutine(CheckQuestItem());
            return true;
        }
        return false;
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
                RemoveItemByID(id, 1);
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

    private List<int> _itemIDlist;
    int num = 0;
    private void Start()
    {
        _itemIDlist = new();
        foreach (int q in _craft._itemDictionary.ItemDic.Keys)
        {
            _itemIDlist.Add(q);
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
        
        // 디버그용 코드
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            num++;
            if (num == _itemIDlist.Count) num = 0;
            Debug.Log($"{_itemIDlist[num]}  {_craft._itemDictionary.ItemDic[_itemIDlist[num]].Name}");
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            num--;
            if (num == -1) num = _itemIDlist.Count - 1;
            Debug.Log($"{_itemIDlist[num]}  {_craft._itemDictionary.ItemDic[_itemIDlist[num]].Name}");
        }
        if (Input.GetKeyDown(KeyCode.Home))
        {
            AddItemByID(_itemIDlist[num], 1);
            Debug.Log($"아이템 지급됨");
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            RemoveItemByID(_itemIDlist[num], 1);
            Debug.Log($"아이템 지움");
        }
    }
    public ItemBase GetHandItem(int hand)
    {
       return _model.InvItems[_controller.EquippedSlotIndex[hand]].Data;
    }
}
