using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMS_TEMPMOVE : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D 또는 좌/우 화살표
        float verticalInput = Input.GetAxis("Vertical");   // W/S 또는 상/하 화살표

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
