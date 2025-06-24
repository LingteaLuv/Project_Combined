using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HingeDoorOpen : MonoBehaviour
{
    [SerializeField] private DoorType _doorType;
    [SerializeField] private float _openAngle = 90f;
    [SerializeField] private float _duration = 1f;

    private Quaternion _openedRotation;
    private Quaternion _closedRotation;

    private Key _key;
    private bool _isOpen = false;
    private bool _isOnRotated = false;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _closedRotation = transform.rotation;
        _openedRotation = _closedRotation * Quaternion.Euler(0, _openAngle * (_doorType == DoorType.RotateRight ? -1 : 1), 0);
    }

    public void Toggle(List<Key> playerKeys)
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
    private void TryOpen(List<Key> playerKeys)
    {
        if (_key == null)
        {
            Debug.Log("TryOpen : 그냥 열리는지 체크");
            RotateDoor();
            return;
        }

        if (playerKeys.Any(k => k.KeyId == _key.KeyId))
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
