using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteract : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private float _interactDistance;

    [SerializeField] private KeyCode _interactKey;

    private bool _isInteracted;

    public void Interact(List<Key> playerKeys)
    {
        if (!_isInteracted)
        {
            Vector3 startPos = transform.position;
            Vector3 direction = transform.forward;

            if (Physics.Raycast(startPos, direction, out RaycastHit hit, _interactDistance))
            {
                if (hit.collider.gameObject.layer == 7)
                {
                    Door door = hit.collider.GetComponent<Door>();
                    door.Toggle(playerKeys);
                }
            }
        }
    }
}