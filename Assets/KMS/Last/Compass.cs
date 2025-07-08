using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    private void Update()
    {
        float dot = Vector3.Dot(Vector3.forward, UISceneLoader.Instance.Player.transform.forward);
        float angle = Vector3.Angle(Vector3.right, UISceneLoader.Instance.Player.transform.forward);
        if (dot >= 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
