using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] LayerMask _interactLayer;

    private List<Collider> _colliders = new List<Collider>();

    private Collider _interacting;
    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _colliders.Add(other);
        } 
    }

    private void OnTriggerStay(Collider other)
    {
        if (_colliders.Count == 0) return;
        float distance = float.MaxValue;
        Collider near = null;
        foreach ( Collider c in _colliders)
        {
            float temp = (transform.position - c.transform.position).magnitude;
            if (temp < distance)
            {
                distance = temp;
                near = c;
            }
        }
        if (_interacting == near)
        {
            return;
        }
        else
        {
            
            near.GetComponent<Interactable>().Outlinable.enabled = true;
            _interacting.GetComponent<Interactable>().Outlinable.enabled = false;
            _interacting = near;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _colliders.Remove(other);
        }
        if (_interacting == other)
        {
            _interacting = null;
            other.GetComponent<Interactable>().Outlinable.enabled = false;
        }
    }
}
