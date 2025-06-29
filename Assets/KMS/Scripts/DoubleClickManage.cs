using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class DoubleClickManage : MonoBehaviour
{
    private int _current;
    private Coroutine _cor;
    private WaitForSeconds _wfs;
    private int _count;
    public bool IsExist;

    private void Awake()
    {
        _wfs = new WaitForSeconds(2f);
        _current = -1;
        _count = 0;
    }

    public void StartCo(int clickedNum) // 누르는 시점 호출
    {
        if (_current == clickedNum)
        {
            return;
        }
        _cor = null;
        _current = clickedNum;
        _count = 0;
        IsExist = true; 
        _cor = StartCoroutine(Delay());
    }
    public void EndCo()
    {
        if (IsExist)
        {
            _count++;
        }
        if (_count >= 2)
        {
            StopCoroutine(_cor);
            _cor = null;
            _current = -1;
            _count = 0;
            IsExist = false;
            Debug.Log("double click");
            StartCoroutine(Equip());
        }
    }
    private IEnumerator Delay()
    {
        yield return _wfs;
        _cor = null;
        _current = -1;
        _count = 0;
        IsExist = false;
    }

    private IEnumerator Equip()
    {
        yield return new WaitForEndOfFrame();
        InventoryManager.Instance.Controller.UseETCItemButton(InventoryManager.Instance.Controller.SelectedIndex);
        InventoryManager.Instance.Controller.EquipButton(InventoryManager.Instance.Controller.SelectedIndex);
    }
    public void TryEquip()
    {
        InventoryManager.Instance.Controller.UseETCItemButton(InventoryManager.Instance.Controller.SelectedIndex);
        InventoryManager.Instance.Controller.EquipButton(InventoryManager.Instance.Controller.SelectedIndex);
    }
}
