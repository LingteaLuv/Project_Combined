using UnityEngine;

public class TestScript : MonoBehaviour
{
    Camera cam;
    public LayerMask targetLayer;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, targetLayer))
            {
                Debug.Log(hit.collider.gameObject.name);
                Lootable lb = hit.collider.GetComponentInChildren<Lootable>();
                if (lb != null)
                {
                    lb.IsLootable = !lb.IsLootable;
                    Debug.Log(lb.IsLootable);
                }
            }
        }
    }
}