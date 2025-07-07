using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawn : MonoBehaviour
{
    [Header("Please in zombie and playr prefab")] 
    [SerializeField] private GameObject[] _spawnPos;
    [SerializeField] private GameObject[] _zombiePrefabs;
    [SerializeField] private MonsterSpawnData _data;
    
    private int _spawnCount;
    private NavMeshAgent _agent;
    private Dictionary<int, GameObject> _zombieDic;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _zombieDic = new Dictionary<int, GameObject>();
        for (int i = 0; i < _zombiePrefabs.Length; i++)
        {
            _zombieDic.Add(_zombiePrefabs[i].GetComponent<Monster_temp>().Info.EnemyID, _zombiePrefabs[i]);
        }
        _data.SetList();
      
    }

    private int SetWeight()
    {
        List<int> weights = new List<int>();
        for (int j = 0; j < 7; j ++)
        {
            if (_data._dictionary[j][0] <= TimeManager.Instance.DayCount)
            {
                weights.Add(_data._dictionary[j][2]);
            }
        }

        int volume = weights.Count;
        
        int[] weightSums = new int[volume];

        weightSums[0] = weights[0];
        for (int i = 1; i < volume; i++)
        {
            weightSums[i] = weightSums[i - 1] + weights[i];
        }
        
        int rndNum = Random.Range(0, weightSums[volume - 1]);
        
        for (int i = 0; i < volume; i++)
        {
            if (rndNum < weightSums[i]) return _data._enemyIds[i];
        }

        return -1;
    }
    
    private void OnTimeOfDaySpawnMonster(DayTime timeOfDay)
    {
        if (timeOfDay == DayTime.Morning)
        {
            SpawnMonster();
        }
    }

    private void SpawnMonster()
    {
        for (int i = 0; i < _spawnPos.Length; i++)
        {
            int index = SetWeight();
            if (index < 0 || !_zombieDic.ContainsKey(index))
            {
                Debug.Log($"인덱스 이상 : {index}");
                continue;
            }

            GameObject zombiePrefab = Instantiate(_zombieDic[index],_spawnPos[i].transform.position,_spawnPos[i].transform.rotation);
            zombiePrefab.GetComponent<Monster_temp>().SpawnPointLink = _spawnPos[i].transform;
            Debug.Log($"좀비 : {_zombieDic[index].name}, 위치 : {_spawnPos[i].transform.position}");
        
            _agent = zombiePrefab.GetComponent<NavMeshAgent>();
            if (_agent == null || !_agent.isOnNavMesh && _spawnCount < 5)
            {
                Destroy(zombiePrefab);
                _spawnCount++;
                SpawnMonster();
            }

            if (_spawnCount == 5)
            {
                //Debug.Log($"위치 : {_spawnPos[i].transform.position} 좀비 생성 실패");
            }

            _spawnCount = 0;
        }
    }

    private void Start()
    {
        TimeManager.Instance.CurrentTimeOfDay.OnChanged += OnTimeOfDaySpawnMonster;
        OnTimeOfDaySpawnMonster(TimeManager.Instance.CurrentTimeOfDay.Value);
    }
}
