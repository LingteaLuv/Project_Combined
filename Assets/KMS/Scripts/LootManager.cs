using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{

    [SerializeField] private GameObject _f;
    [SerializeField] private GameObject _lootTable;

    private Stack<GameObject> _stack;
    private void Awake()
    {
        SetInstance();
        _stack = new Stack<GameObject>();
    }


    public void NewLootableChecked(Lootable _lootable)
    {
        _stack.Clear();
        _stack.Push(_f);
        SetUI();
    }

    public void SetUI()
    {
        if (_stack == null) return;
        foreach( GameObject s in _stack)
        {
            if (s == _stack.Peek())
            {
                s.SetActive(true);
            }
            else
            {
                s.SetActive(false);
            }
        }
    }

    public void LootableNotExist()
    {
        _stack.Clear();
        _f.SetActive(false);
        _lootTable.SetActive(false);
    }
}
