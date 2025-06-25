using UnityEngine;
public class LootItems : MonoBehaviour
{

    public ItemBase[] ItemDatas = new ItemBase[6];
    public int[] ItemAmounts = new int[6];
    public bool[] ItemBlocked = new bool[6];

    public Item[] Items;
    [Range(0,1)]
    public float Percentage;

    private void Awake()
    {
        Items = new Item[6];
        for (int i = 0; i < Items.Length; i++)
        {
            float p = UnityEngine.Random.Range(0f, 1f);
            if (p > Percentage) ItemDatas[i] = null;
            if (ItemDatas[i] == null) continue;
            ItemBlocked[i] = true;
            Items[i] = new Item(ItemDatas[i]);
            Items[i].SetCount(ItemAmounts[i]);
            float a = UnityEngine.Random.Range(0.6f, 1.0f);
            Items[i].SetDur((int)(Items[i].MaxDurability * a));
        }
    }
}