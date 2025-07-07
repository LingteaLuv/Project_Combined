using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

public class MonsterSpawn : MonoBehaviour
{

    [Header("Please in zombie and playr prefab")] 
    [SerializeField] private GameObject[] _spawnPos;
    
    public GameObject[] ZombiePrefabs;
    private NavMeshAgent _agent;
    private bool _isSpawn = false;
    private List<int> _weightSums;
    
    
    
    private MonsterSpawnData _data;
    private int _spawnCount;
    private GameObject _selectPrefab;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _data.SetList();
        _weightSums = new List<int>();
    }
    // 1일차에는
    // day가 1인 것들만
    // 가중치를 더하고
    // 가중치를 더한 것들을 비교해서
    // 아이템 선택
    // 2일차에는
    // day가 2이하인 것들만 더해서
    // 가중치를 더하고
    // 가중치를 더한 것들을 비교해서
    // 아이템 선택
    private void SetWeight()
    {
        int volume = _data._enemyIds.Length;
        int weights = 0;
        for (int j = 0; j < volume; j++)
        {
            if (_data._dictionary[j][0] <= TimeManager.Instance.DayCount)
            {
                weights += _data._dictionary[j][1];
            }
            _weightSums.Add += 
        }
        
        for (int i = 0; i < volume; i++)
        {
            for (int j = 0; j < volume; j++)
            {
                if (_data._dictionary[j][0] <= TimeManager.Instance.DayCount)
                {
                    weightSums[i] += _data._dictionary[j][1];
                }
            }
        }
        int rndNum = Random.Range(0, weightSums);
        
        for (int i = 0; i < volume; i++)
        {
            if (rndNum < _weightSums) return i < _itemCount ? i : _itemCount;
        }
    }
    
    private void OnTimeOfDaySpawnMonster(DayTime timeOfDay)
    {
        if (timeOfDay == DayTime.Morning)
        {
            SpawnMonster();
        }
    }

    private void SetMonster()
    {
        
    }
    
    private void SpawnMonster()
    {
        for (int i = 0; i < _spawnPos.Length; i++)
        {
            GameObject zombiePrefab = Instantiate(_selectPrefab);
            zombiePrefab.transform.position = _spawnPos[i].transform.position;
            _agent = zombiePrefab.GetComponent<NavMeshAgent>();
            if (_agent == null || !_agent.isOnNavMesh && _spawnCount < 5)
            {
                Destroy(zombiePrefab);
                _spawnCount++;
                SpawnMonster();
            }
            if (_spawnCount == 5)
            {
                Debug.Log($"위치 : {_spawnPos[i].transform.position} 좀비 생성 실패");\
            }
            _spawnCount = 0;
        }
    }

    private void Start()
    {
        // 일단 구독하고
        TimeManager.Instance.CurrentTimeOfDay.OnChanged += OnTimeOfDaySpawnMonster;
        OnTimeOfDaySpawnMonster(TimeManager.Instance.CurrentTimeOfDay.Value);
    }
}
