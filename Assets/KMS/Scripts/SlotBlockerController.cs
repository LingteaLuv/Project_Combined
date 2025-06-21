using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBlockerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Coroutine _coroutine;


    private void OnDisable()
    {
        _animator.SetBool("IsPointerDown", false);
    }
    public void PointerDown()
    {
        _coroutine = StartCoroutine(WaitAndPrint());
        _animator.SetBool("IsPointerDown", true);
    }
    public void PointerUp()
    {
        StopCoroutine(_coroutine);
        _animator.SetBool("IsPointerDown", false);
    }
    private IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(3f);
        LootManager.Instance.RemoveBlocker(GetSlotIndex());
    }
    private int GetSlotIndex()
    {
        int.TryParse(gameObject.name, out int index);
        return index;
    }

}
