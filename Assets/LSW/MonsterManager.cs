using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private List<GameObject> _monsters;
    

    private void Update()
    {
        foreach (var monster in _monsters)
        {
            if (monster == null) continue;
            {
                float distance = Vector3.Distance(_player.transform.position, monster.transform.position);
                bool active = distance < 80f;

                if (monster.activeSelf != active)
                {
                    monster.SetActive(active);
                }
            }
        }
    }

}
