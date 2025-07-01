using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawn : MonoBehaviour
{

    [Header("Please in zombie and playr prefab")]
    public GameObject ZombiePrefab;
    public Transform  SpawnPosition;


    [Header("SpawnRange and Angle")]
    public float SpawnRadius = 6;
    public float MinAngle = -180f; // 좌
    public float MaxAngle = 180f; // 우

    [Header("Spawn Time")]
    public float SpawnTime = 10;
    public float SpawnInterval;

    private NavMeshAgent _agent;


    public void SpawnMonster()
    {
        float monsterSpawnAngle = Random.Range(MinAngle, MaxAngle);
        Quaternion rot = Quaternion.Euler(0, monsterSpawnAngle, 0);
        Vector3 direction = rot * SpawnPosition.forward;
        Vector3 SpawnPose = SpawnPosition.position + direction * SpawnRadius;
        GameObject zombiePrefab = Instantiate(ZombiePrefab, SpawnPose, Quaternion.identity);

        _agent = zombiePrefab.GetComponent<NavMeshAgent>();
        if (_agent == null || !_agent.isOnNavMesh)
        {
            Destroy(zombiePrefab);
        }
    }

    private void Update()
    {
        SpawnTime += Time.deltaTime;
        {
            if (SpawnTime > SpawnInterval)
            {
                SpawnMonster();
                SpawnTime = 0;
            }
        }
    }
}
