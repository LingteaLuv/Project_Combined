using Unity.VisualScripting;
using UnityEngine;
public class LootItems : MonoBehaviour
{

    public ItemBase[] ItemDatas = new ItemBase[6];          
    public int[] ItemAmounts = new int[6];
    public bool[] ItemBlocked = new bool[6];
    public Item[] Items;

    public LootInitType InitType = LootInitType.Fixed;
    public int[] Percentages = new int[6];

    private void Awake()
    {
        Items = new Item[6];
        if (InitType == LootInitType.Fixed)
        {
            FixedInit();
        }
        else
        {
            RandomInit();
        }
    }

    private void FixedInit()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (ItemDatas[i] == null) continue;
            ItemBlocked[i] = true;
            Items[i] = new Item(ItemDatas[i]);
            Items[i].SetCount(ItemAmounts[i]);
            float a = UnityEngine.Random.Range(0.6f, 1.0f);
            Items[i].SetDur((int)(Items[i].MaxDurability * a));
        }
    }
    private void RandomInit()
    {
        int index = 0;
        for (int i = 0; i < Items.Length; i++)
        {
            int sel = Random.Range(1, 101);
            int sum = 0;
            for(int j = 0; j < ItemDatas.Length; j++)
            {
                if (ItemDatas[j] == null) continue;
                if (sel > sum && sel <= sum + Percentages[j])
                {
                    Debug.Log(index);
                    ItemBlocked[index] = true;
                    Items[index] = new Item(ItemDatas[j]);
                    Items[index].SetCount(ItemAmounts[index]);
                    float a = UnityEngine.Random.Range(0.6f, 1.0f);
                    Items[index].SetDur((int)(Items[index].MaxDurability * a));
                    index++;
                    break;
                }
                else
                {
                    sum += Percentages[j];
                }
            }
        }
    }
    public enum LootInitType
    {
        Fixed,
        Random
    }
}