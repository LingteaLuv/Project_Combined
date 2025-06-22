using UnityEngine;
public class LootItems : MonoBehaviour
{

    public ItemBase[] ItemDatas = new ItemBase[6];
    public int[] ItemAmounts = new int[6];
    public bool[] ItemBlocked = new bool[6];

    public Item[] Items;

    private void Awake()
    {
        Items = new Item[6];
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
}