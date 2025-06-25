using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrInverseKinematic : MonoBehaviour
{
    public Transform leftHand;

    private Animator animator;
    private int layerIndex_Weapons;

    void Awake()
    {
        animator = GetComponent<Animator>();
        layerIndex_Weapons = animator.GetLayerIndex("Gun Upper Layer");
    }

    private void OnAnimatorIK(int _layerIndex)
    {
        if (_layerIndex != layerIndex_Weapons)
        {
            return;
        }

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
    }
}
