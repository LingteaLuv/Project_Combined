
using UnityEngine;


public class Lootable : MonoBehaviour
{
    [SerializeField] private OutlineDrawer _outlinable;
    public LootItems LootItems;
    public OutlineDrawer Outlinable { get { return _outlinable; } }

    //[SerializeField] private FUIController _FUIController;
    public FUIController FUIController { get; set; }


    [SerializeField] public bool IsLootable;


    private void Awake()
    {
        _outlinable = GetComponentInParent<OutlineDrawer>();
        FUIController = GetComponent<FUIController>();
        OffOutline();
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
