
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHandItemController : MonoBehaviour
{
    [SerializeField] public Transform Right;
    [SerializeField] public Transform Left;
    [SerializeField] public PrefabListSO Prefabs;
    [SerializeField] private PlayerAttack _playerAttack;
    
    public Dictionary<int, GameObject> PrefabIDs { get; set; } //id를 통해 장비 프리팹을 불러오기 위함

    public GameObject CurrentLeftItem;
    public GameObject CurrentRightItem;

    private InventoryController _control;
    private InventoryModel _model;
    private Animator _animator;

    private GunWeaponBase _gwb;

    private void Awake()
    {
        _control = GetComponent<InventoryController>();
        _model = GetComponent<InventoryModel>();
        PrefabIDs = new();
        for (int i = 0; i < Prefabs.IDList.Count; i++)
        {
            PrefabIDs.Add(Prefabs.IDList[i], Prefabs.List[i]);
        }
    }
    private void Start()
    {
        _playerAttack = UISceneLoader.Instance.Playerattack;
        _animator = _playerAttack.GetComponent<Animator>();
        Right = _playerAttack._right_Hand_target.transform;
        Left = _playerAttack._left_Hand_target.transform;
    }

    //public void Subscribe(HandType type, ItemHolder holder)
    //{
    //    if (type == HandType.left)
    //    {
    //        Left = holder.gameObject;
    //    }
    //    else
    //    {
    //        Right = holder.gameObject;
    //    }
    //}

    public void HoldItem(HandType type, int id)
    {
        GameObject temp;
        if (type == HandType.left)
        {
            temp = PrefabIDs[id];
            CurrentLeftItem = Instantiate(temp, Left);
        }
        else
        {
            temp = PrefabIDs[id];
            CurrentRightItem = Instantiate(temp, Right);
        }
    }

    public void UpdateItems()
    {
        DeholdBoth();
        int rightIndex = _control.EquippedSlotIndex[0];
        int leftIndex = _control.EquippedSlotIndex[1];
        if ((rightIndex == leftIndex) && _model.InvItems[rightIndex] == null) return;
        if (rightIndex == -1) //오른손 들린게 없다
        {
            StartCoroutine(UW()); // 아무것도 안들렸다는 신호를 보낼 것.
            if (leftIndex != -1) //왼손은 들렸다.
            {
                HoldItem(HandType.left, _model.InvItems[leftIndex].Data.ItemID);
                return;
            }
            return;
        }
        HoldItem(HandType.right, _model.InvItems[rightIndex].Data.ItemID);
        if (_model.InvItems[rightIndex].Data.Type == ItemType.Gun) //들린게 총이면
        {
            GunWeaponBase _gwb = CurrentRightItem.GetComponent<GunWeaponBase>();
            //_gwb.currentammocount = _model.InvItems[rightIndex].CurrentAmmoCount; (아니면 _model.InvItems[rightIndex]) 그대로 넘김
        }
        StartCoroutine(UW());

        // 왼손에 들린게 없거나 두손무기라면 스킵 방패라면 들어주고,
        if (leftIndex == -1)
        {
            return;
        }
        if (leftIndex == rightIndex)
        {
            return;
        }
        HoldItem(HandType.left, _model.InvItems[leftIndex].Data.ItemID);



    }
    private IEnumerator UW() // 약간 지연 필요
    {
        yield return new WaitForEndOfFrame();
        if (_model.InvItems[_control.EquippedSlotIndex[0]].Data.Type == ItemType.Gun) //만약 들린게 총이면
        {
            CurrentRightItem.GetComponent<GunWeaponBase>()._item = _model.InvItems[_control.EquippedSlotIndex[0]]; //Item정보를 줌(현재탄약수등)
        }
        
        //생성된 프리팹에 정보를 넘겨주
        _playerAttack.UpdateWeapon(); //생성된 프리팹에서 정보를 받음
    }
    public void DeholdItem(HandType type)
    {
        if (type == HandType.left)
        {
            if (CurrentLeftItem != null)
            {
                Destroy(CurrentLeftItem);
                CurrentLeftItem = null;
            }
        }
        else
        {
            if (CurrentRightItem != null)
            {
                Destroy(CurrentRightItem);
                CurrentRightItem = null;
            }
        }
    }
    public void DeholdBoth()
    {
        DeholdItem(HandType.left);
        DeholdItem(HandType.right);
    }

    public void AnimationLoad(Item toEquip)
    {
        bool isBeforeitemNull;
        Item before = null;
        if (_control.EquippedSlotIndex[0] == -1)
        {
            isBeforeitemNull = true;
        }
        else
        {
            before = _model.InvItems[_control.EquippedSlotIndex[0]];
            if (before == toEquip) return;
            isBeforeitemNull = before == null ? true : false;
        }

        if (toEquip == null && isBeforeitemNull) //빈손 > 빈손 아무것도 안함
        {
            Debug.Log("빈손 에서 빈손");
        }
        else if ((toEquip == null) && !isBeforeitemNull) // 장비 > 빈손
        {
            ItemType temp = before.Data.Type;
            if (temp == ItemType.Melee || temp == ItemType.Special || temp == ItemType.Gun)
            {
                Debug.Log("무기 에서 빈손");
                _animator.SetTrigger("UnEquip");
            }
        }
        else if (toEquip != null && isBeforeitemNull) // 빈손 > 장비
        {
            ItemType temp = toEquip.Data.Type;
            if (temp == ItemType.Melee || temp == ItemType.Special || temp == ItemType.Gun)
            {
                Debug.Log("빈손 에서 무기");
                _animator.SetTrigger("Equip");
            }
        }
        else // 무기 > 무기
        {
            ItemType temp = toEquip.Data.Type;
            if (temp == ItemType.Melee || temp == ItemType.Special || temp == ItemType.Gun)
            {
                Debug.Log("무기 에서 무기");
                _animator.SetTrigger("Equip");
            }
        }
    }
}
