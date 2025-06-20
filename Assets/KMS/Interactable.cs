using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPOOutline;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Outlinable _outlinable;
    public Outlinable Outlinable { get { return _outlinable; } }

    private void Awake()
    {
        _outlinable.enabled = false;
    }


}
