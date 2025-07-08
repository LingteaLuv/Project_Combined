using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField] private GameObject _player;
    [SerializeField] public List<GameObject> _monsters;

    private float _timer = 0;
    protected override bool ShouldDontDestroy => false;
    public void Unregister(GameObject monster)
    {
        _monsters.Remove(monster);
    }
    
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

        _timer += Time.deltaTime;
        if (_timer > 5f)
        {
            _monsters.RemoveAll(m => m == null);
            _timer = 0f;
        }
    }
}
