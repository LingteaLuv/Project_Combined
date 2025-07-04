using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotBlockerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Coroutine _coroutine;


    private void OnDisable()
    {
        _animator.SetBool("IsPointerDown", false);
    }


    public void PointerDown(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Right) return;
        _coroutine = StartCoroutine(WaitAndPrint());
        _animator.SetBool("IsPointerDown", true);
        LootManager.Instance.CurrentBlockerIndex = GetSlotIndex();
    }
    public void PointerUp(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Right) return;
        StopCoroutine(_coroutine);
        _animator.SetBool("IsPointerDown", false);
        LootManager.Instance.CurrentBlockerIndex = -1;
    }

    public void Cancel()
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
