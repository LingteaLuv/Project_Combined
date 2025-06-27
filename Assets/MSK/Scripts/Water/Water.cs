using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] PlayerMovement _playerMove;

    /*  시야 중력 조절용 변수
    [SerializeField] private float _waterDrag;
    [SerializeField] private float _waterFogDensity;
    [SerializeField] private Color _waterColor;

    private float _originDrag;
    private float _originFogDensity;
    private Color _originColor;
    */
    private void Start()
    {
        //_originColor = RenderSettings.fogColor;
        //_originFogDensity = RenderSettings.fogDensity;
        //_originDrag = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetWater(Collider _player)
    {
        _playerMove.SetWater(true);

        //  물속 시야, 중력 조절
        /*
        _player.transform.GetComponent<Rigidbody>().drag = _waterDrag;

        RenderSettings.fogColor = _waterColor;
        RenderSettings.fogDensity = _waterFogDensity;
        */
    }
    private void GetOutWater(Collider _player)
    {
        if (_playerMove.IsWater)
        {
            _playerMove.SetWater(false);

        //  물속 시야, 중력 조절
        /*
            _player.transform.GetComponent<Rigidbody>().drag = _originDrag;

            RenderSettings.fogColor = _originColor;
            RenderSettings.fogDensity = _originFogDensity;
        */
        }
    }
}
