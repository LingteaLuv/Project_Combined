using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abd : MonoBehaviour
{

    public Outlinable ol;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(1);
        ol.enabled = false;
        StartCoroutine(co());
    }

    public IEnumerator co()
    {
        while (true)
        {

            yield return new WaitForSeconds(1f);
            ol.enabled = true;
            yield return new WaitForSeconds(1f);
            ol.enabled = false;
        }
    }

}
