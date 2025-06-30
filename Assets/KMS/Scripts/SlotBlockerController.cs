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
        LootManager.Instance.CurrentBlockerIndex = GetSlotIndex();
    }
    public void PointerUp()
    {
        StopCoroutine(_coroutine);
        _animator.SetBool("IsPointerDown", false);
        LootManager.Instance.CurrentBlockerIndex = -1;
    }
    private IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(1f);
        LootManager.Instance.RemoveBlocker(GetSlotIndex());
    }
    private int GetSlotIndex()
    {
        int.TryParse(gameObject.name, out int index);
        return index;
    }

}
