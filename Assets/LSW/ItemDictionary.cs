using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDictionary : Singleton<ItemDictionary>
{
    [SerializeField] public List<ItemBase> Items;
    
    [SerializeField] public List<Recipe> Recipes;
    public Dictionary<int, ItemBase> ItemDic { get; private set; }
    public Dictionary<int, Recipe> RecipeDic { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
#if UNITY_EDITOR
    private void Init()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            int key = Items[i].ItemID;
            ItemDic.Add(key,Items[i]);
        }
        
        for (int i = 0; i < Recipes.Count; i++)
        {
            int key = Recipes[i].ItemID;
            RecipeDic.Add(key,Recipes[i]);
        }
    }
#endif
}
