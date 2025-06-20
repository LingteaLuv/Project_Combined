using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private PlayerMovement _pm;


    // �÷��̾� �̵� �Է� ����
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
        // �̵�
        Vector3 inputDir = GetInputDir();
        _pm.SetMove(inputDir);

        // ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _pm.Jump();
        }
        
    }
}
