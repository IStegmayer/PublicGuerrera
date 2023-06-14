using System.Collections;
using System.Collections.Generic;
using Dzajna;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isUsingRightHand = "isUsingRightHand";
    public bool isUsingRightHandStatus;

    public string isUsingLeftHand = "isUsingRightHand";
    public bool isUsingLeftHandStatus;

    public string isInvulnerable = "isInvulnerable";
    public bool isInvulnerableStatus;

    public string isInteractingBool = "isInteracting";
    public bool isInteractingStatus;

    public string isFiringSpellBool = "isFiringSpell";
    public bool isFiringSpellStatus;

    public string isRotatingWithRM = "isRotatingWithRM";
    public bool isRotatingWithRootMotionStatus;

    public string canRotateBool = "canRotate";
    public bool canRotateStatus = true;

    public string isMirroredBool = "isMirrored";
    public bool isMirroredStatus;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var character = animator.GetComponent<CharacterManager>();

        character.IsUsingLeftHand = false;
        character.IsUsingRightHand = false;
        character.IsAttacking = false;
        character.IsBeingBackstabbed = false;
        character.IsBeingRiposted = false;
        character.IsPerformingBackstab = false;
        character.IsPerformingRiposte = false;
        character.CanBeParried = false;
        character.CanBeRiposted = false;

        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
        animator.SetBool(isRotatingWithRM, isRotatingWithRootMotionStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isInvulnerable, isInvulnerableStatus);
        animator.SetBool(isMirroredBool, isMirroredStatus);
    }
}