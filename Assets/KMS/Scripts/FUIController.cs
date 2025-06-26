
using UnityEngine;
using UnityEngine.UI;

public class FUIController : MonoBehaviour
{
    [SerializeField] private Transform _f;
    private Transform _camera;
    [SerializeField] public Image Dark;
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
    public void OnFUI()
    {
        _f.gameObject.SetActive(true);
    }
    public void OffFUI()
    {
        _f.gameObject.SetActive(false);
    }
}
