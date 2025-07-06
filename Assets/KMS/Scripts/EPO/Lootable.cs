
using UnityEngine;
using EPOOutline;
using System.Collections;

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
    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return null;
        for (int i = 0; i < 6; i++)
        {
            if (LootItems.Items[i] != null) yield break;
        }
        IsLootable = false;
        if (DestroyAfterLooting) // 루팅 완료 시 파괴
        {
            Lootable temp = this;
            if (temp.After != null) //다음에 전환할 것이 있음
            {
                Vector3 posOffset = temp.transform.root.position + Vector3.up * 0.23f;
                GameObject g = Instantiate(temp.After, posOffset, temp.transform.root.rotation);
                g.SetActive(true);
            }
            Destroy(temp.transform.root.gameObject);
        }
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
