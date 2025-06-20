using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // �÷��̾� �̵� �Է� ����
    public Vector3 GetInputDir()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        return new Vector3(x, 0, z).normalized;
    }
}
