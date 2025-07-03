using UnityEngine;

public class Water : MonoBehaviour
{
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
        if (other.CompareTag("Player"))
        {
            var move = other.GetComponent<PlayerMovement>();
            if (move != null)
            {
                move.SetWater(true);
                Debug.Log("물 진입");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var move = other.GetComponent<PlayerMovement>();
            if (move != null)
            {
                move.SetWater(false);
                Debug.Log("물 나옴");
            }
        }
    }
    private void GetWater(Collider _player)
    {
        var move = _player.GetComponent<PlayerMovement>();
        move.SetWater(true);

        Debug.Log("물 진입");
        //  물속 시야, 중력 조절
        /*
        _player.transform.GetComponent<Rigidbody>().drag = _waterDrag;

        RenderSettings.fogColor = _waterColor;
        RenderSettings.fogDensity = _waterFogDensity;
        */
    }
    private void GetOutWater(Collider _player)
    {
        var move = _player.GetComponent<PlayerMovement>();
        if (move.IsWater)
        {
            move.SetWater(false);
            Debug.Log("물 나옴");
            //  물속 시야, 중력 조절
            /*
                _player.transform.GetComponent<Rigidbody>().drag = _originDrag;

                RenderSettings.fogColor = _originColor;
                RenderSettings.fogDensity = _originFogDensity;
            */
        }
    }
}
