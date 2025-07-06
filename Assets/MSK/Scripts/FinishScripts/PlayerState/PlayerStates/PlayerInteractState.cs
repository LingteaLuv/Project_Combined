using System.Collections;
using UnityEngine;

public class PlayerInteractState : PlayerState
{
    public PlayerInteractState(PlayerStateMachine fsm, PlayerMovement movement)
        : base(fsm, movement) { }

    public override void Enter()
    {
        if (HasInteractable(out IInteractable target))
        {
            _movement.Controller.PlayInteractAnimation();
            _movement.StartCoroutine(InteractRoutine(target));
        }
        else
        {
            _fsm.ChangeState(_movement.Controller.IdleState);
        }
    }
    public override void Exit() { }

    public override void FixedTick() { }

    public override void Tick() { }

    private IEnumerator InteractRoutine(IInteractable target)
    {
        yield return new WaitForSeconds(0.3f);
        target.Interact();

        _movement.Controller.StopInteractAnimation();
        _fsm.ChangeState(_movement.Controller.IdleState);
    }

    private bool HasInteractable(out IInteractable target)
    {
        Vector3 origin = _movement.transform.position + Vector3.up * 1.0f;
        Vector3 direction = _movement.transform.forward;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.layer.Equals(7))
            {
                target = hit.collider.GetComponentInParent<DoorInteractable>();
                return true;
            }
        }
        target = null;
        return false;
    }
}