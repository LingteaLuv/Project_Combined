using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBgms : MonoBehaviour
{
    public bool InGameBGM = true;

    void Update()
    {
        AudioManager.Instance.PlayBgms(InGameBGM);
    }
}
