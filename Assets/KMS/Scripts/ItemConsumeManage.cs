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
    private IConsumeHandler _consume;

    private void Awake()
    {
        _control = GetComponent<InventoryController>();
        _craft = GetComponent<CraftingController>();
        _model = GetComponent<InventoryModel>();
        _consume = UISceneLoader.Instance.Playerattack.GetComponent<IConsumeHandler>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Consume();
        }
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            Debug.Log(_model.InvItems[_control.EquippedSlotIndex[0]].CurrentAmmoCount);
            Debug.Log((_model.InvItems[_control.EquippedSlotIndex[0]].Data as GunItem).AmmoCapacity);
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Reload();
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            _control.ReduceEquippedItem(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.ScrollLock))
        {
            _model.InvItems[_control.EquippedSlotIndex[0]].CurrentAmmoCount -= 1;
        }
    }
    public void Consume() // 착용한 소모품을 아예 지워버림
    {
        //현재 오른손 아이템 확인해서 소모템이면 지우고, 착용템 사라졌을때 처리, 효과 적용
        if (UIManager.Instance.IsUIOpened.Value) return;
        if (_control.EquippedSlotIndex[0] == -1) return; //오른손 할당없음
        Item item = _model.InvItems[_control.EquippedSlotIndex[0]];
        if (item == null) return; //오른손 빈칸임
        if (item.Data.Type != ItemType.Consumable) return; //소모품 아님
        _consume.Consume(item.Data);
        _control.RemoveEquippedItem(0); //오른손 아이템 지움
    }

    public bool Reload() //현재 오른손에 든 총의 리로드 개수 만큼 있으면 지우고 장전, 없으면 false
    {
        int MaxCount = (_model.InvItems[_control.EquippedSlotIndex[0]].Data as GunItem).AmmoCapacity;
        int CurCount = _model.InvItems[_control.EquippedSlotIndex[0]].CurrentAmmoCount;
        int need = MaxCount - CurCount;
        if (need == 0) return false;
        if (_craft.RemoveItemByID((_model.InvItems[_control.EquippedSlotIndex[0]].Data as GunItem).AmmoID, need))
        {
            _model.InvItems[_control.EquippedSlotIndex[0]].SetAmmoCount(MaxCount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UseEatButton(int index)
    {
        Item exist = _model.InvItems[index];
        if (exist.Data.Type != ItemType.Consumable) return false; // 이후 장비아이템에 대한 equipbutton실행될듯
        _control.RemoveSelectedItem();
        _consume.Consume(exist.Data);
        return true;


    }

}
