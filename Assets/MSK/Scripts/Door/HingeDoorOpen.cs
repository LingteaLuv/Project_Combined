using System.Collections;
using UnityEngine;

public class HingeDoorOpen : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorType _doorType;
    [SerializeField] private float _openAngle = 90f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private bool _isOpen = false;  // 문의 잠금 여부

    [SerializeField] private ItemBase _key;         // 문의 열쇠정보
    
    private Quaternion _openedRotation;
    private Quaternion _closedRotation;

    private bool _isOnRotated = false;              // 문이 열렸는지의 여부

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        float closedY = (_doorType == DoorType.RotateRight) ? 180f : 0f;
        Vector3 euler = transform.eulerAngles;

        // RotateRight일 때만 Z이동 보정
        if (_doorType == DoorType.RotateRight)
        {
            float zSize = 0f;
            BoxCollider boxCol = GetComponent<BoxCollider>();
            MeshCollider meshCol = GetComponent<MeshCollider>();
            if (boxCol != null)
            {
                zSize = boxCol.size.z * transform.localScale.z;
            }
            else if (meshCol != null)
            {
                zSize = meshCol.bounds.size.z;
            }

            if (zSize > 0)
            {
                transform.position += -transform.forward * zSize;
            }
        }

        // 회전 적용
        euler.y = closedY;
        transform.eulerAngles = euler;

        _closedRotation = transform.rotation;
        float openAngle = _doorType == DoorType.RotateRight ? _openAngle : -_openAngle;
        _openedRotation = _closedRotation * Quaternion.Euler(0, openAngle, 0);
    }
    public void Interact()
    {
        Toggle(_key);
    }

    public void Toggle(ItemBase playerKeys)
    {
        if (_isOnRotated) return;
        if (!_isOpen)
        {
            TryOpen(playerKeys);
        }
        else
        {
            Close();
        }
    }

    private void TryOpen(ItemBase playerKeys)
    {
        if (_key == null)
        {
            Debug.Log("TryOpen : 그냥 열리는지 체크");
            RotateDoor();
            return;
        }

        if (InventoryManager.Instance.FindItemByID(_key.ItemID))
        {
            {
                Debug.Log("TryOpen : 열쇠 보유 체크");
                _isOnRotated = true;
                switch (_doorType)
                {
                    case DoorType.RotateRight:
                        RotateDoor();
                        break;
                    case DoorType.RotateLeft:
                        RotateDoor();
                        break;
                    case DoorType.Slide:
                        SlideOpen();
                        break;
                }
            }
        }
        else
        {
            Debug.Log("TryOpen : 열쇠 미보유");
        }
    }

    private void Close()
    {
        _isOnRotated = true;
        switch (_doorType)
        {
            case DoorType.RotateRight:
                RotateDoor();
                break;
            case DoorType.RotateLeft:
                RotateDoor();
                break;
            case DoorType.Slide:
                SlideOpen();
                break;
        }
    }
    private void SlideOpen()
    {
        // 슬라이드 문 여는 메서드
    }
    private void RotateDoor()
    {
        Quaternion rotation = _isOpen ? _closedRotation : _openedRotation;
        StartCoroutine(RotateRoutine(rotation));
    }

    private IEnumerator RotateRoutine(Quaternion rotation)
    {
        Quaternion startRotation = transform.rotation;
        float timer = 0f;

        while (timer < _duration)
        {
            float t = timer / _duration;
            transform.rotation = Quaternion.Slerp(startRotation, rotation, t);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.rotation = rotation;
        _isOpen = (rotation == _openedRotation);
        _isOnRotated = false;
    }
}

public enum DoorType
{
    RotateRight, RotateLeft, Slide
}
