using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.XR;
using UnityEngine.UIElements;
using static Codice.CM.Common.Purge.PurgeReport;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryModel _model;

    [SerializeField] private InventoryRenderer _renderer;

    public bool IsHolding;
    public int HoldingIndex;
    public int NextIndex;

    public int SelectedIndex;
    private int _beforeSelectedIndex;

    public int[] EquippedSlotIndex;

    private void Awake()
    {
        IsHolding = false;
        HoldingIndex = -1;
        NextIndex = -1;
        SelectedIndex = -1;
        _beforeSelectedIndex = -1;

        EquippedSlotIndex = new int[] { -1, -1 };
    }




    private bool[] GetFlags(ItemBase item)
    {
        bool[] flags = new bool[18]; //false 인 경우에만 들어갈 수 있음

        if (item.Type == ItemType.ETC || item.Type == ItemType.Stuff)
        {
            flags[0] = true;
            flags[1] = true;
            flags[2] = true;
            flags[3] = true;
            flags[4] = true;
            flags[5] = true;
        }
        return flags;
    }

    private void Swap(int a, int b)
    {
        Item tempItem = _model.InvItems[a];
        _model.InvItems[a] = _model.InvItems[b];
        _model.InvItems[b] = tempItem;
    }
    public void EquipButton(int index) //해당 인벤토리 칸 아이템(선택된)을 장착시도
    {
        Item exist = _model.InvItems[index];
        if (EquippedSlotIndex[0] == -1 && EquippedSlotIndex[1] == -1) // 선택된게 아예없다면
        {
            for (int i= 0; i < 6; i++) // 빈 칸 추척
            {
                if (_model.InvItems[i] == null)
                {
                    _model.InvItems[i] = _model.InvItems[index];
                    _model.InvItems[index] = null;
                    Equip(i);
                    _renderer.RenderInventory();
                    return;
                }
            } // 위에서 리턴안됨 -> 꽉차 있으니 첫칸과 스왑
            Swap(0, index);
            Equip(0);
        }
        else if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[1] != -1)
        {
            if (EquippedSlotIndex[0] == EquippedSlotIndex[1]) // 두손장비가 선택된상태
            {
                Swap(EquippedSlotIndex[0], index);
                Equip(EquippedSlotIndex[0]);

            }
            else // 한손장비 두개 선택된상태
            {
                if (exist.Data.Type == ItemType.Melee)
                {
                    Swap(EquippedSlotIndex[0], index);
                    Equip(EquippedSlotIndex[0]);

                }
                else if (exist.Data.Type == ItemType.Shield)
                {
                    Swap(EquippedSlotIndex[1], index);
                    Equip(EquippedSlotIndex[1]);
                }
                else
                {
                    int min = EquippedSlotIndex.Min();
                    Swap(min, index);
                    Equip(min);
                }
            }
        }
        else if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[1] == -1) //무기칸에만 있음 (방패만 선택됨)
        {
            Swap(EquippedSlotIndex[0], index);
            Equip(EquippedSlotIndex[0]);
        }
        else if (EquippedSlotIndex[0] == -1 && EquippedSlotIndex[1] != -1) //방패칸에만 있음 (무기만 선택됨)
        {
            Swap(EquippedSlotIndex[1], index);
            Equip(EquippedSlotIndex[1]);
        }
        _renderer.RenderInventory();

    }
    public void Equip(int index) //해당 칸 아이템에 대한 장착 시도
    {
        Item exist = _model.InvItems[index];
        if (exist == null) return;
        if (exist.Data.Type == ItemType.Consumable)
        {
            return;
        }
        else if (exist.Data.Type == ItemType.Melee)
        {
            if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[0] == EquippedSlotIndex[1]) //양손장비가 선택되어 있는 상황
            {
                EquippedSlotIndex[1] = -1;
            }
            EquippedSlotIndex[0] = index;

        }
        else if (exist.Data.Type == ItemType.Shield)
        {
            if (EquippedSlotIndex[1] != -1 && EquippedSlotIndex[0] == EquippedSlotIndex[1]) //양손장비가 선택되어 있는 상황
            {
                EquippedSlotIndex[0] = -1;
            }
            EquippedSlotIndex[1] = index;

        }
        else if (exist.Data.Type == ItemType.Gun)
        {
            EquippedSlotIndex[0] = index;
            EquippedSlotIndex[1] = index;

        }
        else if (exist.Data.Type == ItemType.Special)
        {
            EquippedSlotIndex[0] = index;
            EquippedSlotIndex[1] = index;

        }
        _renderer.RenderEquip(EquippedSlotIndex);

    }
    private void AutoEquip(int index) // 아무것도 선택되어 있지 않거나, 
    {
        Item exist = _model.InvItems[index];

        if (EquippedSlotIndex[0] == -1 && EquippedSlotIndex[1] == -1) // 아무 것도 선택되어있지 않다면
        {
            Equip(index); //선택
            return;
        }
        if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[1] != -1) //양손무기가 있거나, 무기, 방패 둘다 선택됨
        {
            return; //못넣음
        }
        else if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[1] == -1) //무기칸에만 있음 (방패만 선택됨)
        {
            if (exist.Data.Type == ItemType.Shield) Equip(index);
        }
        else if (EquippedSlotIndex[0] == -1 && EquippedSlotIndex[1] != -1) //방패칸에만 있음 (무기만 선택됨)
        {
            if (exist.Data.Type == ItemType.Melee) Equip(index);
        }

    }
    public bool AddItem(ItemBase item, int amount, int durability)
    {
        int a = amount; // 넣을 개수
        List<int> itemExist = new List<int>();
        List<int> nullindexs = new List<int>();
        bool[] flags = GetFlags(item);

        int MaxStack = 1;
        if (item is EtcItem) MaxStack = (item as EtcItem).MaxStackSize;
        for (int i = 0; i < _model.SlotCount; i++)
        {
            if (flags[i]) continue; // 아이템 종류에 따른 flag 스킵
            if (_model.InvItems[i] == null) // 빈 공간 인덱스들 저장함
            {
                nullindexs.Add(i);
                continue;
            }
            if (_model.InvItems[i].Data == item && _model.InvItems[i].StackCount < MaxStack) //같은 아이템이 존재하는데, 최대 개수보다 부족함
            {
                int temp = MaxStack - _model.InvItems[i].StackCount; //부족한 수량
                if (temp == a) // 부족한 개수와 넣을 개수가 같음
                {
                    if (itemExist.Count > 0)
                    {
                        foreach( int j in itemExist) _model.InvItems[j].StackCount = MaxStack;
                    }
                    _model.InvItems[i].StackCount = MaxStack;
                    return true;
                }
                else if (temp > a) // 부족한 개수가 넣을 개수보다 많음
                {
                    if (itemExist.Count > 0)
                    {
                        foreach (int j in itemExist) _model.InvItems[j].StackCount = MaxStack;
                    }
                    _model.InvItems[i].StackCount += a;
                    return true;
                }
                else //부족한 개수가 넣을 개수보다 적음 (남음)
                {
                    itemExist.Add(i); // 해당 인덱스 기록
                    a -= temp;
                }
            }
        }
        // 위에서 return 안됨 -> 중복된 아이템들 위에 모두 중첩시켜도 아이템이 남는 상황 -> 빈 공간 활용
        // 중첩시켰을 경우 남는다고 가정한 아이템의 개수가 a에 저장됨
        if (nullindexs.Count > 0) //빈 공간이 존재함
        {
            int temp = a;
            List<int> addables = new List<int>();
            for (int i = 0; i < nullindexs.Count; i++)
            {
                addables.Add(nullindexs[i]);
                temp -= MaxStack; // 각 칸마다 최대 개수식 빼본다.
                if (temp > 0) // 그래도 남는다면
                {
                    if (i == nullindexs.Count - 1) // 현재 마지막 빈 공간을 조사중?
                    {
                        return false; // 모든 빈 공간을 채워도 넣을 수 없음
                    }
                    continue;
                }
                else // 
                {
                    break;
                }
            }
            temp += MaxStack; // 마지막 빈 공간에 들어갈 개수
            Debug.Log(temp);
            if (itemExist.Count > 0) // 우선 스택가능한 영역 채움
            {
                foreach (int j in itemExist) _model.InvItems[j].StackCount = MaxStack;
            }
            for (int i = 0; i < addables.Count; i++) // 이후 빈 공간에 채워넣음
            {
                if (i == addables.Count - 1) // 마지막 부분
                {
                    _model.InvItems[addables[i]] = new Item(item);
                    _model.InvItems[addables[i]].StackCount = temp;
                    _model.InvItems[addables[i]].Durability = durability;
                }
                else
                {
                    _model.InvItems[addables[i]] = new Item(item);
                    _model.InvItems[addables[i]].StackCount = MaxStack;
                    _model.InvItems[addables[i]].Durability = durability;


                }
            }
            if (addables[0] < 6 && item.Type != ItemType.Consumable)
            {
                AutoEquip(addables[0]); // 소모품이 아니고, 아이템이 들어온 첫 번째 칸이 퀵슬롯에 포함되어 있을 경우, 퀵슬롯 인덱스
            }
            return true;
        }
        else // 빈 공간도 없음 -> 못넣음
        {
            return false;
        }
    }

    public void HoldItem(int index)
    {
        if (_model.InvItems[index] == null) return;
        if (index == EquippedSlotIndex[0] || index == EquippedSlotIndex[1]) return;
        _renderer.HoldRender(index);
        IsHolding = true;
        HoldingIndex = index;
        NextIndex = HoldingIndex;
    }

    public void CancelHolding()
    {
        if (IsHolding)
        {
            IsHolding = false;
            HoldingIndex = -1;
            NextIndex = -1;
            _renderer.HoldClear();
        }
    }

    public void SelectSlot(int index)
    {
        _beforeSelectedIndex = SelectedIndex;
        if (index < 6) // 퀵슬롯 내 슬롯을 선택하려고 할 경우 선택 취소
        {
            SelectedIndex = -1;
        }
        else
        {
            SelectedIndex = index;
        }
        _renderer.SelectRender(_beforeSelectedIndex, SelectedIndex);
    }
    public void PutItem()
    {
        if (!IsHolding) return;
        bool[] flags = GetFlags(_model.InvItems[HoldingIndex].Data);
        if (flags[NextIndex] || NextIndex == EquippedSlotIndex[0] || NextIndex == EquippedSlotIndex[1])
        {
            CancelHolding();
        }
        else if (NextIndex == HoldingIndex)
        {
            SelectSlot(HoldingIndex);
        }
        else if (_model.InvItems[NextIndex] == null)
        {
            PlaceItem();
            Debug.Log("place");
        }
        else
        {
            SwitchItem();
            Debug.Log("replace");
        }
        CancelHolding();
        _renderer.RenderInventory();
    }

    public void PlaceItem()
    {
        _model.InvItems[NextIndex] = _model.InvItems[HoldingIndex];
        _model.InvItems[HoldingIndex] = null;
        if (SelectedIndex == HoldingIndex) SelectSlot(NextIndex);

    }
    public void SwitchItem()
    {
        bool[] flags = GetFlags(_model.InvItems[NextIndex].Data);
        if (flags[HoldingIndex])
        {
            CancelHolding();
            return;
        }
        Item tempItem = _model.InvItems[NextIndex];
        _model.InvItems[NextIndex] = _model.InvItems[HoldingIndex];
        _model.InvItems[HoldingIndex] = tempItem;
        if (SelectedIndex == HoldingIndex) 
        {
            SelectSlot(NextIndex);
        }
        else if (SelectedIndex == NextIndex)
        {
            SelectSlot(HoldingIndex);
        }

    }

    public void RemoveSelectedItem()
    {
        _model.InvItems[SelectedIndex] = null;
        SelectSlot(-1);
        _renderer.RenderInventory();
    }
}
 