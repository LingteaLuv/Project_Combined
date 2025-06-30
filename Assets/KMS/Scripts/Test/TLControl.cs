using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class TLControl : MonoBehaviour
{

    [SerializeField] PlayableDirector _pd;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _pd.Play();
        }
    }
}
