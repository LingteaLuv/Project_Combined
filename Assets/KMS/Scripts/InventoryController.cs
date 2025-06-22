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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) InventoryManager.Instance.ToggleInventory();
        //if (Input.GetKeyDown(KeyCode.Alpha0)) AddItem(_model.ItemList.ItemList[0]);
        //if (Input.GetKeyDown(KeyCode.Alpha1)) AddItem(_model.ItemList.ItemList[1]);
        //if (Input.GetKeyDown(KeyCode.Alpha2)) AddItem(_model.ItemList.ItemList[2]);
        //if (Input.GetKeyDown(KeyCode.Alpha3)) AddItem(_model.ItemList.ItemList[3]);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Add(_model.ItemList.ItemList[6], 2, 30);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Add(_model.ItemList.ItemList[6], 2, 10);
        InventoryManager.Instance.HoldSlot.transform.position = Input.mousePosition;
    }

    //public bool AddItem(ItemSO item, int amount)
    //{
    //    int nullindex = -1;
    //    for (int i = 0; i < _model.SlotCount; i++)
    //    {
    //        if (_model.InvItems[i] == item && _model.InvItemAmounts[i] < item.MaxInventoryAmount) //중첩 가능성있음
    //        {
    //
    //            _model.InvItemAmounts[i]++;
    //            if (InventoryManager.Instance.IsinventoryOpened) _renderer.RenderInventory();
    //            return true;
    //        }
    //        if (_model.InvItems[i] == null && nullindex == -1) // 빈공간 확보
    //        {
    //            nullindex = i;
    //        }
    //    }
    //    if (nullindex != -1)
    //    {
    //        _model.InvItems[nullindex] = item;
    //        _model.InvItemAmounts[nullindex]++;
    //        if (InventoryManager.Instance.IsinventoryOpened) _renderer.RenderInventory();
    //        return true;
    //    }
    //    return false;
    //}

    public void RemoveItem(ItemSO item)
    {
        for (int i = 0; i < _model.SlotCount; i++)
        {
            if (_model.InvItems[i] == item)
            {
                RemoveItemByIndex(i);
                return;
            }
        }
    }

    public void RemoveItemByIndex(int index)
    {
        _model.InvItemAmounts[index]--;
        if (_model.InvItemAmounts[index] <= 0)
        {
            _model.InvItemAmounts[index] = 0;
            _model.InvItems[index] = null;
        }
        if (InventoryManager.Instance.IsinventoryOpened) _renderer.RenderInventory();
    }

    public void Add(ItemSO item, int amount, int durability)
    {
        AddItem(item, amount, durability);
        _renderer.RenderInventory();
    }
    public bool AddItem(ItemSO item, int amount, int durability)
    {
        int a = amount; // 넣을 개수
        List<int> itemExist = new List<int>();
        List<int> nullindexs = new List<int>();
        for (int i = 0; i < _model.SlotCount; i++)
        {
            if (_model.InvItems[i] == item && _model.InvItemAmounts[i] < item.MaxInventoryAmount) //같은 아이템이 존재하는데, 최대 개수보다 부족함
            {
                int temp = item.MaxInventoryAmount - _model.InvItemAmounts[i]; //부족한 수량
                if (temp == a) // 부족한 개수와 넣을 개수가 같음
                {
                    if (itemExist.Count > 0)
                    {
                        foreach( int j in itemExist) _model.InvItemAmounts[j] = item.MaxInventoryAmount;
                    }
                    _model.InvItemAmounts[i] = item.MaxInventoryAmount;
                    return true;
                }
                else if (temp > a) // 부족한 개수가 넣을 개수보다 많음
                {
                    if (itemExist.Count > 0)
                    {
                        foreach (int j in itemExist) _model.InvItemAmounts[j] = item.MaxInventoryAmount;
                    }
                    _model.InvItemAmounts[i] += a;
                    return true;
                }
                else //부족한 개수가 넣을 개수보다 적음 (남음)
                {
                    itemExist.Add(i); // 해당 인덱스 기록
                    a -= temp;
                }
            }
            if (_model.InvItems[i] == null) // 빈 공간 인덱스들 저장함
            {
                nullindexs.Add(i);
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
            if (itemExist.Count > 0)
            {
                foreach (int j in itemExist) _model.InvItemAmounts[j] = item.MaxInventoryAmount;
            }
            for (int i = 0; i < addables.Count; i++)
            {
                _model.InvItems[addables[i]] = item;
                _model.InvItemDurabilitys[addables[i]] = durability;
                if (i == addables.Count - 1) // 마지막 부분
                {
                    _model.InvItemAmounts[addables[i]] = temp;
                }
                else
                {
                    _model.InvItemAmounts[addables[i]] = item.MaxInventoryAmount;

                }
            }
            return true;
        }
        else // 빈 공간도 없음 -> 못넣음
        {
            return false;
        }
    }

    //public void HandleItem(int index)
    //{
    //    if (_model.HeldItem == null)
    //    {
    //        if (_model.InvItems[index] == null) return;
    //        HoldItem(index);
    //    }
    //    else
    //    {
    //        if (_model.HeldItem == _model.InvItems[index] && 
    //            _model.InvItemAmounts[index] < _model.InvItems[index].MaxInventoryAmount)
    //        {
    //            
    //            CombineItem(index);
    //        }
    //        else if (_model.InvItems[index] == null)
    //        {
    //            PlaceItem(index);
    //        }
    //        else
    //        {
    //            ReplaceItem(index);
    //        }
    //    }
    //    _renderer.RenderInventory();
    //}

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
    //public void CombineItem(int index)
    //{
    //    int sum = _model.HeldItemAmount + _model.InvItemAmounts[index];
    //    if (sum <= _model.InvItems[index].MaxInventoryAmount)
    //    {
    //        _model.InvItemAmounts[index] = sum;
    //        _model.HeldItemAmount = 0;
    //        _model.HeldItem = null;
    //    }
    //    else
    //    {
    //        _model.InvItemAmounts[index] = _model.InvItems[index].MaxInventoryAmount;
    //        _model.HeldItemAmount = sum - _model.InvItems[index].MaxInventoryAmount;
    //    }
    //}

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
        _model.InvItemAmounts[NextIndex] = _model.InvItemAmounts[HoldingIndex];
        _model.InvItemDurabilitys[NextIndex] = _model.InvItemDurabilitys[HoldingIndex];
        _model.InvItems[HoldingIndex] = null;
        _model.InvItemAmounts[HoldingIndex] = 0;
        _model.InvItemDurabilitys[HoldingIndex] = 0;
    }
    public void SwitchItem()
    {
        ItemSO tempItem = _model.InvItems[NextIndex];
        int tempAmount = _model.InvItemAmounts[NextIndex];
        int tempDur = _model.InvItemDurabilitys[NextIndex];
        _model.InvItems[NextIndex] = _model.InvItems[HoldingIndex];
        _model.InvItemAmounts[NextIndex] = _model.InvItemAmounts[HoldingIndex];
        _model.InvItemDurabilitys[NextIndex] = _model.InvItemDurabilitys[HoldingIndex];
        _model.InvItems[HoldingIndex] = tempItem;
        _model.InvItemAmounts[HoldingIndex] = tempAmount;
        _model.InvItemDurabilitys[HoldingIndex] = tempDur;
    }
}
 