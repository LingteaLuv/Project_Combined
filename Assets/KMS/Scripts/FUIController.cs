using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FUIController : MonoBehaviour
{
    [SerializeField] private Transform _f;
    private Transform _camera;

    public Image Dark { get; set; }
    public Image Fill { get; set; }

    private void Start()
    {
        _camera = Camera.main.transform;
    }
    private void Update()
    {
        _f.forward = _camera.forward;
    }
    public void OnDark()
    {
        Dark.enabled = true;
    }
    public void OffDark()
    {
        Dark.enabled = false;
    }
}
