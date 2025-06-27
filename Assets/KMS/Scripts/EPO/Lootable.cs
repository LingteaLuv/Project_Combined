
using UnityEngine;
using EPOOutline;

public enum LootableType
{
    box,
    pickup
}

public class Lootable : MonoBehaviour
{
    
    private LootItems _lootItems;
    public LootItems LootItems { get { return _lootItems; } }
    private Outlinable _outlinable;
    public Outlinable Outlinable { get { return _outlinable; } }

    private FUIController _FUIController;
    public FUIController FUIController { get { return _FUIController; } }

    [SerializeField] public GameObject After;
    [SerializeField] public bool IsLootable;
    [SerializeField] public LootableType Type;
    [SerializeField] public bool DestroyAfterLooting;


    private void Awake()
    {
        _lootItems = GetComponent<LootItems>();
        _outlinable = GetComponentInParent<Outlinable>();
        _FUIController = GetComponent<FUIController>();
        OffOutline();
        if (After != null) After.SetActive(false);
    }

    public void OnOutline()
    {
        _outlinable.enabled = true;
    }
    public void OffOutline()
    {
        _outlinable.enabled = false;
    }


}
