using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Drag&Drop")] [SerializeField] private GameObject _triggerZombie;
    [SerializeField] private GameObject _wall;

    public bool IsOnTank;
    public bool HasLootedOnTank;
    protected override bool ShouldDontDestroy => false;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        LootManager.Instance.AfterLoot += LootOnTank; //randomgrid를 루트했을 때만 호출됨 => 좀비
    }

    private void LootOnTank()
    {
        if (IsOnTank) // 물탱크 위에 있다면
        {
            HasLootedOnTank = true;
        }
    }

    private void Init()
    {
        //_triggerZombie.GetComponent<Monster>().OnDeath.AddListener(VigilanteScenePlay)
    }

    private void VigilanteScenePlay()
    {
        // 자경단 이벤트 씬 실행 메서드
    }

}
