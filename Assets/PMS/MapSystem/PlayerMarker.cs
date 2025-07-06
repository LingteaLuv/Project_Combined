using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
