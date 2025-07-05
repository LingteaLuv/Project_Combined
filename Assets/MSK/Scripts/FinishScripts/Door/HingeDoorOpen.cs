using System.Collections;
using UnityEngine;
public class HingeDoorOpen : MonoBehaviour, IInteractable
{
    [Header("Door Config")]
    [SerializeField] private DoorType _doorType = DoorType.RotateRight;
    [SerializeField] private float _openAngle = 90f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Transform _leftPos;
    [SerializeField] private Transform _rightPos;

    [Header("회전할 오브젝트")]
    [SerializeField] public Transform _parentToRotate;



    [Header("Door Key (optional)")]
    [SerializeField] private ItemBase _key;

    private bool _isOpen = false;
    private bool _isRotating = false;

    private float _closedY;
    private float _openedY;

    private void Awake()
    {
        SetPivotPositionsFromParent();
        InitAngles();
    }

    /// <summary>
    /// 부모 오브젝트의 바운드 기준으로 양 끝(좌/우) 피벗 위치 자동 배치
    /// </summary>
    private void SetPivotPositionsFromParent()
    {
        if (_parentToRotate == null || _leftPos == null || _rightPos == null)
        {
            Debug.LogWarning("Parent, LeftPos, RightPos 중 할당 안 된 것이 있습니다.");
            return;
        }

        Bounds parentBounds = GetBounds(_parentToRotate);

        // Y와 Z는 부모의 중심, X만 min/max로 분기 (일반적인 회전문 기준)
        Vector3 leftWorld = new Vector3(parentBounds.min.x, _parentToRotate.position.y, _parentToRotate.position.z);
        Vector3 rightWorld = new Vector3(parentBounds.max.x, _parentToRotate.position.y, _parentToRotate.position.z);

        _leftPos.position = leftWorld;
        _rightPos.position = rightWorld;
    }

    /// <summary>
    /// 부모의 바운드(BoxCollider → MeshRenderer → 자식 MeshRenderer → 포지션 fallback)
    /// </summary>
    private Bounds GetBounds(Transform t)
    {
        var box = t.GetComponent<BoxCollider>();
        if (box != null) return box.bounds;
        var mesh = t.GetComponent<MeshRenderer>();
        if (mesh != null) return mesh.bounds;

        var children = t.GetComponentsInChildren<MeshRenderer>();
        if (children.Length > 0)
        {
            Bounds b = children[0].bounds;
            for (int i = 1; i < children.Length; i++)
                b.Encapsulate(children[i].bounds);
            return b;
        }
        return new Bounds(t.position, Vector3.one * 0.1f);
    }

    /// <summary>
    /// 열기/닫기 각도 초기화
    /// </summary>
    private void InitAngles()
    {
        _closedY = _parentToRotate.eulerAngles.y;
        _openedY = (_doorType == DoorType.RotateRight) ?
            _closedY + _openAngle : _closedY - _openAngle;
    }

    public void Interact() => Toggle(_key);

    public void Toggle(ItemBase playerKeys)
    {
        if (_isRotating) return;
        if (!_isOpen)
            TryOpen(playerKeys);
        else
            StartCoroutine(RotateDoor(false));
    }

    private void TryOpen(ItemBase playerKeys)
    {
        if (_key == null || InventoryManager.Instance.FindItemByID(_key.ItemID))
        {
            StartCoroutine(RotateDoor(true));
            _key = null;
        }
        else
            Debug.Log("열쇠가 없습니다.");
    }

    /// <summary>
    /// 문을 회전시키는 코루틴 (회전축은 부모의 좌/우 끝)
    /// </summary>
    private IEnumerator RotateDoor(bool open)
    {
        _isRotating = true;
        Transform pivot = (_doorType == DoorType.RotateRight) ? _rightPos : _leftPos;

        float from = _parentToRotate.eulerAngles.y;
        float to = open ? _openedY : _closedY;
        float delta = Mathf.DeltaAngle(from, to);

        float elapsed = 0f;
        while (elapsed < _duration)
        {
            float t = elapsed / _duration;
            float currentY = from + delta * t;
            float step = currentY - _parentToRotate.eulerAngles.y;
            _parentToRotate.RotateAround(pivot.position, Vector3.up, step);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // 마지막 각도 보정
        float fix = Mathf.DeltaAngle(_parentToRotate.eulerAngles.y, to);
        _parentToRotate.RotateAround(pivot.position, Vector3.up, fix);

        _isOpen = open;
        _isRotating = false;
    }
    public bool CanOpen()
    {
        if (_key == null) return true;
        return InventoryManager.Instance.FindItemByID(_key.ItemID, false);
    }
}

public enum DoorType 
{ 
    RotateRight, RotateLeft, Slide
}
