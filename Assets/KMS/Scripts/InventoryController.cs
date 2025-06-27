
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System;


public class InventoryController : MonoBehaviour
{
    private InventoryModel _model;

    private InventoryRenderer _renderer;

    private CraftingController _crafting;

    private PlayerHandItemController _hand;

    public bool IsHolding;
    public int HoldingIndex;
    public int NextIndex;

    public int SelectedIndex;
    private int _beforeSelectedIndex;

    public int[] EquippedSlotIndex;



    private void Awake()
    {
        _model = GetComponent<InventoryModel>();
        _renderer = GetComponent<InventoryRenderer>();
        _crafting = GetComponent<CraftingController>();
        _hand = GetComponent<PlayerHandItemController>();
        IsHolding = false;
        HoldingIndex = -1;
        NextIndex = -1;
        SelectedIndex = -1;
        _beforeSelectedIndex = -1;

        EquippedSlotIndex = new int[] { 0, 0 };
    }

    private void Start()
    {
        _renderer.RenderEquip(EquippedSlotIndex);
    }
    private bool[] GetFlags(ItemBase item)
    {
        bool[] flags = new bool[18]; //false 인 경우에만 들어갈 수 있음

        if (item.Type == ItemType.ETC || item.Type == ItemType.Material)
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
    public void Dur(int index, int amount) // 지정된 손 아이템 내구도 감소시킴
    {
        if (EquippedSlotIndex[index] == -1) return; //해당 손에 아무것도 없음
        Item target = _model.InvItems[EquippedSlotIndex[index]]; //그 손에 들린 아이템
        if (target == null) return; // 빈 공간을 들고있음
        if (target.MaxDurability == -1) return; //손의 아이템이 내구도가 존재하지않음
        target.SetDur(target.Durability - amount);
        if (target.Durability <= 0)
        {
            RemoveEquippedItem(index);
        }
        else
        {
            _renderer.UpdateDur(EquippedSlotIndex[index]);
        }

    }

    public void RemoveEquippedItem(int index) // 왼손 또는 오른손 아이템 삭제
    {
        int temp = EquippedSlotIndex[index];
        RemoveItem(temp);
        UnEquipAfterRemove(EquippedSlotIndex[index]);
        _renderer.RenderInventory();
    }
    private void Swap(int a, int b)
    {
        Item tempItem;
        if (_model.InvItems[a] == null)
        {
            tempItem = null;
        }
        else
        {
            tempItem = _model.InvItems[a];
        }
        _model.InvItems[a] = _model.InvItems[b];
        _model.InvItems[b] = tempItem;
    }

    public void UseETCItemButton(int index)
    {
        Item exist = _model.InvItems[index];
        if (exist.Data.Type != ItemType.ETC) return; // 이후 장비아이템에 대한 equipbutton실행될듯
        TextManager.Instance.MemoPopUpText($"{exist.Data.ItemID}");
    }
    
    public void EquipButton(int index) //해당 인벤토리 칸 아이템(선택된)을 장착시도
    {
        Item exist = _model.InvItems[index];
        if (exist.Data.Type == ItemType.ETC) return;
        if (EquippedSlotIndex[0] == -1 && EquippedSlotIndex[1] == -1) // 선택된게 아예없다면
        {
            for (int i= 0; i < 6; i++) // 빈 칸 추척
            {
                if (_model.InvItems[i] == null)
                {
                    _model.InvItems[i] = _model.InvItems[index];
                    _model.InvItems[index] = null;
                    Equip(i, false);
                    SelectSlot(index); //껴준 후 기존칸 선택 시도
                    _renderer.RenderInventory();
                    return;
                }
            } // 위에서 리턴안됨 -> 꽉차 있으니 첫칸과 스왑
            Swap(0, index);
            Equip(0, false);
        }
        else if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[1] != -1)
        {
            if (EquippedSlotIndex[0] == EquippedSlotIndex[1]) // 두손장비가 선택된상태 (두손을 사용하는 무언가)
            {
                Swap(EquippedSlotIndex[0], index); //선택된 아이템을 뺀다 (선택된게 없을 경우 예외)
                Equip(EquippedSlotIndex[0], false);

            }
            else // 한손장비 두개 선택된상태 (초록칸이 2개인 상태)
            {
                if (exist.Data.Type == ItemType.Melee)
                {
                    Swap(EquippedSlotIndex[0], index);
                    Equip(EquippedSlotIndex[0], false);

                }
                else if (exist.Data.Type == ItemType.Shield)
                {
                    Swap(EquippedSlotIndex[1], index);
                    Equip(EquippedSlotIndex[1], false);
                }
                else //더 왼쪽에 있는 것에 삽입
                {
                    int min = EquippedSlotIndex.Min();
                    Swap(min, index);
                    Equip(min, false);
                }
            }
        }
        else if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[1] == -1) //무기칸에만 있음 (방패만 선택됨)
        {
            Swap(EquippedSlotIndex[0], index);
            Equip(EquippedSlotIndex[0], false);
        }
        else if (EquippedSlotIndex[0] == -1 && EquippedSlotIndex[1] != -1) //방패칸에만 있음 (무기만 선택됨)
        {
            Swap(EquippedSlotIndex[1], index);
            Equip(EquippedSlotIndex[1], false);
        }
        SelectSlot(index);
        _renderer.RenderInventory();
    }

    public bool FindItem(ItemBase item, bool remove)
    {
        if (_crafting.CountByID[item.ItemID] > 0)
        {
            if (remove)
            {
                RemoveItem(item, 1);
            }
            return true;
        }
        return false;
    }

    public void NewEquip(int index) //해당 칸 아이템 장착
    {

    }
    public void Equip(int index, bool a = true) //해당 칸 아이템에 대한 장착 시도
    {
        Item exist = _model.InvItems[index];
        if (a) _hand.AnimationLoad(exist); //a는 애니 실행 여부
        if (exist == null) // 아무것도 안 든다.
        {
            EquippedSlotIndex[0] = index;
            EquippedSlotIndex[1] = index;
        }
        else if (exist.Data.Type == ItemType.Consumable) //소모품을 든다
        {
            EquippedSlotIndex[0] = index;
            EquippedSlotIndex[1] = index;
        }
        else if (exist.Data.Type == ItemType.Melee) //밀리 무기를 든다
        {
            if (EquippedSlotIndex[1] != -1 && EquippedSlotIndex[0] == EquippedSlotIndex[1]) //양손을 어디서 사용중임 -> 오른손만 가져옴
            {
                EquippedSlotIndex[1] = -1;
            }
            EquippedSlotIndex[0] = index;
            // 밀리 드는 애니메이션 발동

        }
        else if (exist.Data.Type == ItemType.Shield) //방패를 든다
        {
            if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[0] == EquippedSlotIndex[1])
            {
                EquippedSlotIndex[0] = -1;
            }
            EquippedSlotIndex[1] = index;
            // 각자 애니메이션 발동

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
        
         _hand.UpdateItems();
    }

    public void UnEquip(int index)
    {
        //해당 인덱스에 뭔가 장착되어 있는가
        if (EquippedSlotIndex[0] == index)
        {
            if (EquippedSlotIndex[1] == index) //0, 1 둘다 같은 칸을 바라봄 -> 두손에 끼는 것
            {
                EquippedSlotIndex[1] = -1;
            }
            EquippedSlotIndex[0] = -1; //한 손에 낀 것을 뺀 경우
        }
        else if (EquippedSlotIndex[1] == index)
        {
            if (EquippedSlotIndex[0] == index)
            {
                EquippedSlotIndex[0] = -1;
            }
            EquippedSlotIndex[1] = -1;
        }
        else
        {
            return;
        }
        _renderer.RenderEquip(EquippedSlotIndex);
        _hand.UpdateItems();
    }
    public void UnEquipAfterRemove(int index)
    {
        if (EquippedSlotIndex[0] != index && EquippedSlotIndex[1] != index)
        {
            return; //착용된 칸이 아님
        }
        if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[1] != -1)
        {
            if (EquippedSlotIndex[0] == EquippedSlotIndex[1])
            {
                return;
            }
            else
            {
                if (EquippedSlotIndex[0] == index)
                {
                    EquippedSlotIndex[0] = -1;
                }
                else
                {
                    EquippedSlotIndex[1] = -1;
                }
            }
        }
        else if (EquippedSlotIndex[0] == -1 && EquippedSlotIndex[1] != -1)
        {
            EquippedSlotIndex[0] = EquippedSlotIndex[1];
        }
        else if (EquippedSlotIndex[0] != -1 && EquippedSlotIndex[1] == -1)
        {
            EquippedSlotIndex[1] = EquippedSlotIndex[0];
        }
        _renderer.RenderEquip(EquippedSlotIndex);
        _hand.UpdateItems();
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

    public bool RemoveItem(ItemBase item, int amount)// 뺄 개수만큼 뺄 수 없으면 안빼고 false 반환
    {
        int a = amount;
        if (!_crafting.CountByID.TryGetValue(item.ItemID, out int val)) // 해당 아이디의 아이템을 가지고 있는지
        {
            return false;
        }
        else
        {
            if (val < amount) return false;
        }
        //위 경우 아니면 뺄 수 있음
        for (int i = 0; i < _model.SlotCount; i++)
        {
            if (_model.InvItems[i] == null)
            {
                continue;
            }
            else if (_model.InvItems[i].Data == item) // 아이템 존재
            {
                if (_model.InvItems[i].StackCount <= a)
                {
                    a -= _model.InvItems[i].StackCount;
                    if (i < 6) UnEquipAfterRemove(i); // 장비창의 아이템이 빠져나갈 경우 장착 해제
                    _model.InvItems[i] = null;
                }
                else if (_model.InvItems[i].StackCount > a)
                {
                    _model.InvItems[i].StackCount -= a;
                    a = 0;
                }
            }
            if (a == 0)
            {
                break;
            }
        }
        _crafting.CountByID[item.ItemID] -= amount;

        return true;



    }
    public bool AddItem(ItemBase item, int amount, int durability) //넣을 개수만큼 넣을 수 없으면 안넣고 false 반환
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
                    _crafting.Add(item.ItemID, amount);
                    _renderer.RenderInventory();
                    return true;
                }
                else if (temp > a) // 부족한 개수가 넣을 개수보다 많음
                {
                    if (itemExist.Count > 0)
                    {
                        foreach (int j in itemExist) _model.InvItems[j].StackCount = MaxStack;
                    }
                    _model.InvItems[i].StackCount += a;
                    _crafting.Add(item.ItemID, amount);
                    _renderer.RenderInventory();
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
                if (addables[i] == EquippedSlotIndex[0]) //들어간 칸이 현재 손에 든 빈칸이었다면,
                {
                    //해당 칸에 대한 착용
                    Equip(addables[i]);
                }
            }
            //if (addables[0] < 6 && item.Type != ItemType.Consumable)
            //{
            //    AutoEquip(addables[0]); // 소모품이 아니고, 아이템이 들어온 첫 번째 칸이 퀵슬롯에 포함되어 있을 경우, 퀵슬롯 인덱스
            //}
            _crafting.Add(item.ItemID, amount);
            _renderer.RenderInventory();
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
    public void UseSeletedItem()
    {

    }

    public void SelectSlot(int index)
    {
        _beforeSelectedIndex = SelectedIndex;
        if (index < 6 || _model.InvItems[index] == null) // 퀵슬롯 내 슬롯을 선택하려고 할 경우, 또는 선택 위치에 템이없으면
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
        _crafting.CountByID[_model.InvItems[SelectedIndex].Data.ItemID] -= _model.InvItems[SelectedIndex].StackCount;
        _model.InvItems[SelectedIndex] = null;
        SelectSlot(-1);
        _renderer.RenderInventory();
    }
    public void RemoveItem(int index) // 해당 칸 위치의 아이템 지움
    {
        _crafting.CountByID[_model.InvItems[index].Data.ItemID] -= _model.InvItems[index].StackCount;
        _model.InvItems[index] = null;
        _renderer.RenderInventory();
    }



}
 