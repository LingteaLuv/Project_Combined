using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawn : MonoBehaviour
{

    [Header("Please in zombie and playr prefab")]
    public GameObject ZombiePrefab;
    public Transform  SpawnPosition;
    private NavMeshAgent _agent;
    private bool _isSpawn = false;


    private void OnTimeOfDaySpawnMonster(DayTime timeOfDay)
    {
        if (timeOfDay == DayTime.Night && _isSpawn == true)
        {
            SpawnMonster();
        }
    
    }


    public void SpawnMonster()
    {

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
        {
            GameObject zombiePrefab = Instantiate(ZombiePrefab.gameObject, hit.position, Quaternion.identity);

            _agent = zombiePrefab.GetComponent<NavMeshAgent>();

            if (_agent == null || !_agent.isOnNavMesh)
            {
                Destroy(zombiePrefab);
            }

            _isSpawn = false;
        }

   
    }

    private void Start()
    {
        // 일단 구독하고
        TimeManager1.Instance.CurrentTimeOfDay.OnChanged += OnTimeOfDaySpawnMonster;
        OnTimeOfDaySpawnMonster(TimeManager1.Instance.CurrentTimeOfDay.Value);
    }


    private void Update()
    {

    }
}
