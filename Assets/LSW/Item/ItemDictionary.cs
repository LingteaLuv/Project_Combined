using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Dictionary/ItemDictionary")]
public class ItemDictionary : ScriptableObject
{
    [SerializeField] public List<ItemBase> Items;
    
    [SerializeField] public List<Recipe> Recipes;
    
    public Dictionary<int, ItemBase> ItemDic { get; private set; }
    public Dictionary<int, Recipe> RecipeDic { get; private set; }
    
    public void GenerateDic()
    {
        Init();
    }

    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        ItemDic = new Dictionary<int, ItemBase>();
        RecipeDic = new Dictionary<int, Recipe>();
        
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
}
