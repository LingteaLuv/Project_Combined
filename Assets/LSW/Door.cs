using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private DoorType _doorType;
    [SerializeField] private float _openAngle;
    [SerializeField] private float _duration;
    
    private Key _key;
    private bool _isOpen;
    private bool _isOnRotated;

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
        if (playerKeys.Contains(_key))
        {
            _isOnRotated = true;
            switch (_doorType)
            {
                case DoorType.RotateRight:
                    RotateDoor(-1);
                    break;
                case DoorType.RotateLeft:
                    RotateDoor(1);
                    break;
                case DoorType.Slide:
                    SlideOpen();
                    break;
            }
        }
    }
    
    private void Close()
    {
        _isOnRotated = true;
        switch (_doorType)
        {
            case DoorType.RotateRight:
                RotateDoor(1);
                break;
            case DoorType.RotateLeft:
                RotateDoor(-1);
                break;
            case DoorType.Slide:
                SlideOpen();
                break;
        }
    }
    
    private void RotateDoor(int rotateDir)
    {
        Quaternion rotation = Quaternion.Euler(0, rotateDir * _openAngle, 0);
        StartCoroutine(RotateRoutine(rotation));
    }

    private void SlideOpen()
    {
        // 슬라이드 문 여는 메서드
    }
    
    private IEnumerator RotateRoutine(Quaternion rotation)
    {
        Quaternion startRotation = transform.rotation;
        float timer = 0f;
        while (timer < _duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, rotation, timer / _duration);
            yield return null;
        }

        _isOnRotated = false;
    }
}

public enum DoorType
{
    RotateRight, RotateLeft, Slide
}
