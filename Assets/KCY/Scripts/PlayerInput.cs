using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private PlayerMovement _pm;


    // 플레이어 이동 입력 설정
    public Vector3 GetInputDir()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        return new Vector3(x,0,z);
    }


    // Start is called before the first frame update
    private void Awake()
    {
        _pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // 이동
        Vector3 inputDir = GetInputDir();
        _pm.SetMove(inputDir);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _pm.Jump();
        }
        
    }
}
