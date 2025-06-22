using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.XR;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryModel _model;

    [SerializeField] private InventoryRenderer _renderer;

    public bool IsHolding;
    public int HoldingIndex;
    public int NextIndex;

    public int SelectedIndex;
    private int _beforeSelectedIndex;

    private void Awake()
    {
        IsHolding = false;
        HoldingIndex = -1;
        NextIndex = -1;
        SelectedIndex = -1;
        _beforeSelectedIndex = -1;

    }


    private bool[] GetFlags(ItemSO item)
    {
        bool[] flags = new bool[18]; //false 인 경우에만 들어갈 수 있음
        flags[0] = true;
        flags[1] = true;
        flags[2] = true;
        flags[3] = true;
        flags[4] = true;
        flags[5] = true;
        switch (item.Type)
        {
            case ItemType.Weapon:
                flags[0] = false;
                break;
            case ItemType.Shield:
                flags[1] = false;
                break;
            case ItemType.Special:
                flags[2] = false;
                break;
            case ItemType.Consumable:
                flags[3] = false;
                flags[4] = false;
                flags[5] = false;
                break;
        }
        return flags;
    }
    public bool AddItem(ItemSO item, int amount, int durability)
    {
        int a = amount; // 넣을 개수
        List<int> itemExist = new List<int>();
        List<int> nullindexs = new List<int>();
        bool[] flags = GetFlags(item);
        for (int i = 0; i < _model.SlotCount; i++)
        {
            if (flags[i]) continue; // 아이템 종류에 따른 flag 스킵
            if (_model.InvItems[i] == null) // 빈 공간 인덱스들 저장함
            {
                nullindexs.Add(i);
                continue;
            }
            if (_model.InvItems[i].Data == item && _model.InvItems[i].StackCount < item.MaxInventoryAmount) //같은 아이템이 존재하는데, 최대 개수보다 부족함
            {
                int temp = item.MaxInventoryAmount - _model.InvItems[i].StackCount; //부족한 수량
                if (temp == a) // 부족한 개수와 넣을 개수가 같음
                {
                    if (itemExist.Count > 0)
                    {
                        foreach( int j in itemExist) _model.InvItems[j].StackCount = item.MaxInventoryAmount;
                    }
                    _model.InvItems[i].StackCount = item.MaxInventoryAmount;
                    return true;
                }
                else if (temp > a) // 부족한 개수가 넣을 개수보다 많음
                {
                    if (itemExist.Count > 0)
                    {
                        foreach (int j in itemExist) _model.InvItems[j].StackCount = item.MaxInventoryAmount;
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
                temp -= item.MaxInventoryAmount; // 각 칸마다 최대 개수식 빼본다.
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
            temp += item.MaxInventoryAmount; // 마지막 빈 공간에 들어갈 개수
            Debug.Log(temp);
            if (itemExist.Count > 0) // 우선 스택가능한 영역 채움
            {
                foreach (int j in itemExist) _model.InvItems[j].StackCount = item.MaxInventoryAmount;
            }
            for (int i = 0; i < addables.Count; i++) // 이후 빈 공간에 채워넣음
            {
                if (i == addables.Count - 1) // 마지막 부분
                {
                    _model.InvItems[addables[i]] = new Item(item, temp, durability);
                }
                else
                {
                    _model.InvItems[addables[i]] = new Item(item, item.MaxInventoryAmount, durability);

                }
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
        Debug.Log(HoldingIndex);
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
        SelectedIndex = index;
        _renderer.SelectRender(_beforeSelectedIndex, SelectedIndex);
    }
    public void PutItem()
    {
        if (!IsHolding) return;
        Debug.Log("put");
        if (NextIndex == HoldingIndex) ;
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
        IsHolding = false;
        HoldingIndex = -1;
        NextIndex = -1;
        _renderer.HoldClear();
        _renderer.RenderInventory();
    }

    public void PlaceItem()
    {
        _model.InvItems[NextIndex] = _model.InvItems[HoldingIndex];
        _model.InvItems[HoldingIndex] = null;

    }
    public void SwitchItem()
    {
        Item tempItem = _model.InvItems[NextIndex];
        _model.InvItems[NextIndex] = _model.InvItems[HoldingIndex];
        _model.InvItems[HoldingIndex] = tempItem;

    }
}
 