using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSlotControl : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
