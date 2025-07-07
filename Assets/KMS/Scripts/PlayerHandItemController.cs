
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

    [SerializeField] private AnimationClip _equipMotion;
    [SerializeField] private AnimationClip _unequipMotion;

    private Coroutine _anico;
    private WaitForSeconds _equipSc;
    private WaitForSeconds _unequipSc;

    private void Awake()
    {
        _equipSc = new WaitForSeconds(_equipMotion.length);
        _unequipSc = new WaitForSeconds(_unequipMotion.length);
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
        Right = PlayerWeaponManager.Instance._right_Hand_target.transform;
        Left = PlayerWeaponManager.Instance._left_Hand_target.transform;
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

    public void UpdateItemHandle()
    {
        UpdateItems();
        StartCoroutine(UW());
    }

    public void UpdateThrow()
    {
        int rightIndex = _control.EquippedSlotIndex[0];
        if (_model.InvItems[rightIndex] == null)
        {
            StartCoroutine(UW());
            return;
        }
        HoldItem(HandType.right, _model.InvItems[rightIndex].Data.ItemID);
        StartCoroutine(UW());

    }

    public void UpdateItems()
    {
        DeholdBoth();
        int rightIndex = _control.EquippedSlotIndex[0];
        int leftIndex = _control.EquippedSlotIndex[1];
        if ((rightIndex == leftIndex) && _model.InvItems[rightIndex] == null) return;
        if (rightIndex == -1) //오른손 빈 상황
        {
            if (leftIndex != -1) //왼손은 들렸다.
            {
                HoldItem(HandType.left, _model.InvItems[leftIndex].Data.ItemID);
                return; //왼손에만 들린 상황
            }
            return; // 양손 다 텅 빈 상황
        }
        if (_model.InvItems[rightIndex].Data.Type == ItemType.Consumable)
        {
            return;
        }
        HoldItem(HandType.right, _model.InvItems[rightIndex].Data.ItemID);


        // 왼손에 들린게 없거나 두손무기라면 스킵 방패라면 들어주고,
        if (leftIndex == -1)
        {
            return; // 한손무기만 들린 상황
        }
        if (leftIndex == rightIndex)
        {
            return; // 두손무기가 들린 상황
        }
        HoldItem(HandType.left, _model.InvItems[leftIndex].Data.ItemID); //한손무기와 방패가 들린 상황



    }
    private IEnumerator UW() // 약간 지연 필요
    {
        yield return new WaitForEndOfFrame();
        if (_control.EquippedSlotIndex[0] != -1)
        {
            if (_model.InvItems[_control.EquippedSlotIndex[0]] != null)
            {
                if (_model.InvItems[_control.EquippedSlotIndex[0]].Data.Type == ItemType.Gun) //만약 들린게 총이면
                {
                    CurrentRightItem.GetComponent<GunWeaponBase>()._item = _model.InvItems[_control.EquippedSlotIndex[0]]; //Item정보를 줌(현재탄약수등)
                }
            }
        }
        //생성된 프리팹의 정보를 받도록
        PlayerWeaponManager.Instance.UpdateCurrentWeapon();
        //여기서 싱글톤ㅇ 점보 넘겨줌
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
                StartCoroutine(UnequipCoroutine());
            }
        }
        else if (toEquip != null && isBeforeitemNull) // 빈손 > 장비
        {
            ItemType temp = toEquip.Data.Type;
            if (temp == ItemType.Melee || temp == ItemType.Special || temp == ItemType.Gun)
            {
                Debug.Log("빈손 에서 무기");
                _animator.SetTrigger("Equip");
                StartCoroutine(EquipCoroutine());
            }
        }
        else // 무기 > 무기
        {
            ItemType temp = toEquip.Data.Type;
            if (temp == ItemType.Melee || temp == ItemType.Special || temp == ItemType.Gun)
            {
                Debug.Log("무기 에서 무기");
                _animator.SetTrigger("Equip");
                StartCoroutine(EquipCoroutine());
            }
        }
    }

    private IEnumerator EquipCoroutine()
    {
        _playerAttack.IsAttacking = true;
        yield return _equipSc;
        _playerAttack.IsAttacking = false;
    }
    private IEnumerator UnequipCoroutine()
    {
        _playerAttack.IsAttacking = true;
        yield return _unequipSc;
        _playerAttack.IsAttacking = false;
    }
}
