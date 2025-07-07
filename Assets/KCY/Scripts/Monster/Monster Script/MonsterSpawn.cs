using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawn : MonoBehaviour
{

    [Header("Please in zombie and playr prefab")]
    public GameObject[] ZombiePrefabs;
    private NavMeshAgent _agent;
    private bool _isSpawn = false;
    private MonsterSpawnData _data;
    private int spawncount = 0;
    private GameObject _selectPrefab;

    private void OnTimeOfDaySpawnMonster(DayTime timeOfDay)
    {
        if (timeOfDay == DayTime.MidNight)
        {
            //SpawnMon
        }


        if (timeOfDay == DayTime.MidNight && _isSpawn == true)
        {
            //SpawnMonster();
        }
    
    }

    private void SetMonster()
    {
        
    }
    
    /*public void SpawnMonster()
    {
        GameObject zombiePrefab = Instantiate(_selectPrefab);
        _agent = zombiePrefab.GetComponent<NavMeshAgent>();
        if (_agent == null || !_agent.isOnNavMesh && count < 5)
        {
            Destroy(zombiePrefab);
            count++;
            SpawnMonster();
        }
        count = 0;
    }*/

    private void Start()
    {
        // 일단 구독하고
        TimeManager.Instance.CurrentTimeOfDay.OnChanged += OnTimeOfDaySpawnMonster;
        OnTimeOfDaySpawnMonster(TimeManager.Instance.CurrentTimeOfDay.Value);
    }
}
