using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmptyObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _zombies;

    public void Generate()
    {
        for (int i = 0; i < _zombies.Length; i++)
        {
            GameObject empty = new GameObject($"SpawnPos{i}");
            empty.transform.position = _zombies[i].transform.position;
            empty.transform.rotation = _zombies[i].transform.rotation;
        }
    }
}
