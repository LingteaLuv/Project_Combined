
using UnityEngine;


public class InventoryManager : SingletonT<InventoryManager>
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
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Z)) _craft.AddItemByID(1101, 1, 10);
        if (Input.GetKeyDown(KeyCode.X)) _craft.AddItemByID(1102, 1, 10);
        if (Input.GetKeyDown(KeyCode.C)) _craft.AddItemByID(1201, 1, 10);
        if (Input.GetKeyDown(KeyCode.V)) _craft.AddItemByID(1301, 1, 10);
        if (Input.GetKeyDown(KeyCode.B)) _craft.AddItemByID(1401, 1, 10);
        if (Input.GetKeyDown(KeyCode.N)) _craft.AddItemByID(1402, 1, 10);


        if (Input.GetKeyDown(KeyCode.Alpha1)) Controller.Equip(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Controller.Equip(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Controller.Equip(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Controller.Equip(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Controller.Equip(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) Controller.Equip(5);

        if (Input.GetKeyDown(KeyCode.P)) DecreaseWeaponDurability();
        if (Input.GetKeyDown(KeyCode.O)) DecreaseShieldDurability();



        if (Input.GetKeyDown(KeyCode.U)) //사용
         HoldSlot.transform.position = Input.mousePosition;
    }


    public void DecreaseWeaponDurability(int amount = 1)
    {
        _controller.Dur(0, amount);
    }
    public void DecreaseShieldDurability(int amount = 1)
    {
        _controller.Dur(1, amount);
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
}
