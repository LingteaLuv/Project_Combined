using UnityEngine;

public class HingeDoorOpen : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openDuration = 1f;

    private Quaternion _initialRotation;
    private Quaternion _targetRotation;
    private bool _isOpen = false;
    private bool _isMoving = false;

    private void Start()
    {
        _initialRotation = transform.rotation;
        _targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));
    }

    public void Toggle()
    {
        if (_isMoving) return;

        if (_isOpen)
            StartCoroutine(CloseDoorRoutine());
        else
            StartCoroutine(OpenDoorRoutine());
    }

    private System.Collections.IEnumerator OpenDoorRoutine()
    {
        _isMoving = true;

        float time = 0f;
        while (time < openDuration)
        {
            float t = time / openDuration;
            transform.rotation = Quaternion.Slerp(_initialRotation, _targetRotation, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = _targetRotation;
        _isOpen = true;
        _isMoving = false;
    }

    private System.Collections.IEnumerator CloseDoorRoutine()
    {
        _isMoving = true;

        float time = 0f;
        while (time < openDuration)
        {
            float t = time / openDuration;
            transform.rotation = Quaternion.Slerp(_targetRotation, _initialRotation, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = _initialRotation;
        _isOpen = false;
        _isMoving = false;
    }
}
