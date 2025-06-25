using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerHandItemController : MonoBehaviour
{
    [SerializeField] public GameObject Right;
    [SerializeField] public GameObject Left;
    [SerializeField] public PrefabListSO Prefabs;
    public Dictionary<int, GameObject> PrefabIDs { get; set; } //id를 통해 장비 프리팹을 불러오기 위함

    public GameObject CurrentLeftItem;
    public GameObject CurrentRightItem;

    private InventoryController _control;
    private InventoryModel _model;

    private bool IsHoldingTwoHanded => _control.EquippedSlotIndex[0] == _control.EquippedSlotIndex[0];
    private bool IsHoldingSpecial;
    private bool IsHoldingGun;
    private bool IsHoldingNothing;
    private bool IsHoldingOnlyMelee;
    private bool IsHoldingOnlyShield;
    private bool IsHoldingMeleeAndShield;

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

    public void Subscribe(HandType type, ItemHolder holder)
    {
        if (type == HandType.left)
        {
            Left = holder.gameObject;
        }
        else
        {
            Right = holder.gameObject;
        }
    }

    public void HoldItem(HandType type, int id)
    {
        GameObject temp;
        if (type == HandType.left)
        {
            temp = PrefabIDs[id];
            CurrentLeftItem = Instantiate(temp, Left.transform);
        }
        else
        {
            temp = PrefabIDs[id];
            CurrentRightItem = Instantiate(temp, Right.transform);
        }
    }

    public void UpdateItems()
    {
        DeholdBoth();
        int rightIndex = _control.EquippedSlotIndex[0];
        int leftIndex = _control.EquippedSlotIndex[1];
        if (rightIndex == -1)
        {
            if (leftIndex != -1)
            {
                HoldItem(HandType.left, _model.InvItems[leftIndex].Data.ItemID);
                return;
            }
            return;
        }
        HoldItem(HandType.right, _model.InvItems[rightIndex].Data.ItemID);

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

}
