

public class Item
{
    public ItemBase Data { get; private set; }
    public int StackCount { get; set; }
    public int Durability { get; set; }

    public int MaxDurability { get; private set; }
    public int MaxStackSize { get; private set; }

    public int CurrentAmmoCount { get; set; }

    public bool IsStackable => Data.Type == ItemType.Etc || Data.Type == ItemType.Throw;

    //public HandType handType
    //{
    //    get
    //    {
    //
    //    }
    //}

    public Item(ItemBase data)
    {
        Data = data;
        StackCount = 1;
        MaxStackSize = 1;
        Durability = -1;
        MaxDurability = -1;
        CurrentAmmoCount = -1; // 총 전용

        Init();
    }
    private void Init() // 각 클래스에 포함된 값들을 사용하기 쉽게 가져온다.
    {
        switch (Data.Type)
        {
            case ItemType.Etc:
                MaxStackSize = (Data as EtcItem).MaxStackSize;
                break;
            case ItemType.Melee:
                MaxDurability = (Data as MeleeItem).MaxDurability;
                break;
            case ItemType.Shield:
                MaxDurability = (Data as ShieldItem).MaxDurability;
                break;
            case ItemType.Special:
                MaxDurability = (Data as SpecialItem).MaxDurability;
                break;
            case ItemType.Throw:
                MaxStackSize = (Data as ThrowItem).MaxStack;
                break;
            case ItemType.Gun:
                CurrentAmmoCount = 0;
                break;
        }
    }

    public void SetCount(int c)
    {
        if (IsStackable) StackCount = c;
    }
    public void SetDur(int c)
    {
        if (Data.Type == ItemType.Melee || Data.Type == ItemType.Shield || Data.Type == ItemType.Special) Durability = c;
    }

    public void SetAmmoCount(int c)
    {
        if (Data.Type == ItemType.Gun)
        {
            CurrentAmmoCount = c;
        }
    }

    public enum handType
    {
        oneHand,
        twoHand
    }
}
