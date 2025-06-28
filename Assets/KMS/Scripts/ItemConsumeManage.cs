using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumeManage : MonoBehaviour
{
    private InventoryController _control;
    private InventoryModel _model;
    private CraftingController _craft;
    private PlayerProperty _property;

    private void Awake()
    {
        _control = GetComponent<InventoryController>();
        _craft = GetComponent<CraftingController>();
        _model = GetComponent<InventoryModel>();
        _property = UISceneLoader.Instance.Playerattack.GetComponent<PlayerProperty>();
    }
    public void Consume()
    {
        //현재 오른손 아이템 확인해서 소모템이면 지우고, 착용템 사라졌을때 처리, 효과 적용
        if (_control.EquippedSlotIndex[0] == -1) return; //오른손 할당없음
        Item item = _model.InvItems[_control.EquippedSlotIndex[0]];
        if (item == null) return; //오른손 빈칸임
        if (item.Data.Type != ItemType.Consumable) return; //소모품 아님
        //_property.Consume(item.Data); // 효과적용
        _control.RemoveEquippedItem(0); //오른손 아이템 지움
    }

    public bool Reload() //현재 오른손에 든 총의 리로드 개수 만큼 있으면 지우고 장전, 없으면 false
    {
        int MaxCount = (_model.InvItems[_control.EquippedSlotIndex[0]].Data as GunItem).AmmoCapacity;
        int CurCount = _model.InvItems[_control.EquippedSlotIndex[0]].CurrentAmmoCount;
        int need = MaxCount - CurCount;
        if (_craft.RemoveItemByID(9999, need))
        {
            _model.InvItems[_control.EquippedSlotIndex[0]].SetAmmoCount(MaxCount);
            return true;
        }
        else
        {
            return false;
        }
    }

}
