﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dzajna {
public class CharacterAnimatorManager : MonoBehaviour {
    public CharacterManager characterManager;
    public Animator Anim;
    public bool CanRotate;

    // properties hashes
    public int IsGroundedHash { get; private set; }
    public int IsUsingRightHandHash { get; private set; }
    public int IsUsingLeftHandHash { get; private set; }
    public int VerticalHash { get; private set; }
    public int HorizontalHash { get; private set; }
    public int IsInteractingHash { get; private set; }
    public int IsDeadHash { get; private set; }
    public int IsBlockingHash { get; private set; }
    public int CanRotateHash { get; private set; }
    public int IsFiringSpellHash { get; private set; }
    public int IsRotatingWithRMHash { get; private set; }
    public int IsPhaseShiftingHash { get; private set; }
    public int CanDoComboHash { get; private set; }
    public int IsInvulnerableHash { get; private set; }
    public int InAirTimer { get; private set; }
    public int IsChargingAttackHash { get; private set; }
    public int IsTwoHandingWeaponHash { get; private set; }

    // states hashes
    public int RollHash { get; private set; }
    public int BackstepHash { get; private set; }
    public int LandHash { get; private set; }
    public int FallHash { get; private set; }
    public int EmptyHash { get; private set; }
    public int LightAttack1Hash { get; private set; }
    public int LightAttack2Hash { get; private set; }
    public int HeavyAttack1Hash { get; private set; }
    public int HeavyAttack2Hash { get; private set; }
    public int RunningAttackHash { get; private set; }
    public int TH_LightAttack1Hash { get; private set; }
    public int TH_LightAttack2Hash { get; private set; }
    public int TH_HeavyAttack1Hash { get; private set; }
    public int TH_HeavyAttack2Hash { get; private set; }
    public int TH_RunningAttackHash { get; private set; }
    public int WeaponArtHash { get; private set; }
    public int LeftArmIdleHash { get; private set; }
    public int RightArmIdleHash { get; private set; }
    public int TH_IdleHash { get; private set; }
    public int DeathHash { get; private set; }
    public int JumpHash { get; private set; }
    public int FailedCastHash { get; private set; }
    public int BackStabHash { get; private set; }
    public int RiposteHash { get; private set; }
    public int RipostedHash { get; private set; }
    public int BackStabbedHash { get; private set; }
    public int OpenChestHash { get; private set; }
    public int PassThroughFogWallHash { get; private set; }
    public int BlockHash { get; private set; }
    public int GuardBreakHash { get; private set; }
    public int BlockDamage1Hash { get; private set; }
    public int Damage1Hash { get; private set; }
    public int TurnAroundHash { get; private set; }
    public int TurnRightHash { get; private set; }
    public int TurnLeftHash { get; private set; }
    public int PhaseTransitionHash { get; private set; }
    public int ParriedHash { get; private set; }
    public int ParryHash { get; private set; }

    protected virtual void Awake() {
        VerticalHash = Animator.StringToHash("Vertical");
        HorizontalHash = Animator.StringToHash("Horizontal");
        IsInteractingHash = Animator.StringToHash("isInteracting");
        IsDeadHash = Animator.StringToHash("isDead");
        CanRotateHash = Animator.StringToHash("canRotate");
        ParriedHash = Animator.StringToHash("Parried");
        ParryHash = Animator.StringToHash("Parry");
        CanDoComboHash = Animator.StringToHash("canDoCombo");
        IsInvulnerableHash = Animator.StringToHash("isInvulnerable");
        IsRotatingWithRMHash = Animator.StringToHash("isRotatingWithRM");
        TurnAroundHash = Animator.StringToHash("TurnAround");
        TurnLeftHash = Animator.StringToHash("TurnLeft");
        TurnRightHash = Animator.StringToHash("TurnRight");
        PhaseTransitionHash = Animator.StringToHash("PhaseTransition");
        IsPhaseShiftingHash = Animator.StringToHash("isPhaseShifting");
        IsUsingRightHandHash = Animator.StringToHash("isUsingRightHand");
        IsUsingLeftHandHash = Animator.StringToHash("isUsingLeftHand");
        IsBlockingHash = Animator.StringToHash("isBlocking");
        IsFiringSpellHash = Animator.StringToHash("isFiringSpell");
        RollHash = Animator.StringToHash("Roll");
        BackstepHash = Animator.StringToHash("Backstep");
        LandHash = Animator.StringToHash("Land");
        FallHash = Animator.StringToHash("Fall");
        EmptyHash = Animator.StringToHash("Empty");
        Damage1Hash = Animator.StringToHash("Damage1");
        BlockDamage1Hash = Animator.StringToHash("BlockDamage1");
        DeathHash = Animator.StringToHash("Death");
        JumpHash = Animator.StringToHash("Jump");
        FailedCastHash = Animator.StringToHash("FailedCast");
        IsGroundedHash = Animator.StringToHash("isGrounded");
        InAirTimer = Animator.StringToHash("inAirTimer");
        BackStabHash = Animator.StringToHash("BackStab");
        BackStabbedHash = Animator.StringToHash("BackStabbed");
        BlockHash = Animator.StringToHash("Block");
        GuardBreakHash = Animator.StringToHash("GuardBreak");
        RiposteHash = Animator.StringToHash("Riposte");
        RipostedHash = Animator.StringToHash("Riposted");
        OpenChestHash = Animator.StringToHash("OpenChest");
        PassThroughFogWallHash = Animator.StringToHash("PassThroughFogWall");
        LightAttack1Hash = Animator.StringToHash("LightAttack1");
        LightAttack2Hash = Animator.StringToHash("LightAttack2");
        HeavyAttack1Hash = Animator.StringToHash("HeavyAttack1");
        HeavyAttack2Hash = Animator.StringToHash("HeavyAttack2");
        TH_LightAttack1Hash = Animator.StringToHash("TH_LightAttack1");
        TH_LightAttack2Hash = Animator.StringToHash("TH_LightAttack2");
        TH_HeavyAttack1Hash = Animator.StringToHash("TH_HeavyAttack1");
        TH_HeavyAttack2Hash = Animator.StringToHash("TH_HeavyAttack2");
        WeaponArtHash = Animator.StringToHash("WeaponArt");
        LeftArmIdleHash = Animator.StringToHash("LeftArmIdle");
        RightArmIdleHash = Animator.StringToHash("RightArmIdle");
        TH_IdleHash = Animator.StringToHash("TH_Idle");
        IsChargingAttackHash = Animator.StringToHash("isChargingAttack");
        IsTwoHandingWeaponHash = Animator.StringToHash("isTwoHandingWeapon");
        RunningAttackHash = Animator.StringToHash("RunningAttack");
        TH_RunningAttackHash = Animator.StringToHash("TH_RunningAttack");
    }

    public void PlayTargetAnimation(int targetAnim, bool isInteracting, bool canRotate = false,
        bool mirrorAnim = false) {
        Anim.applyRootMotion = isInteracting;
        Anim.SetBool(CanRotateHash, canRotate);
        Anim.SetBool(IsInteractingHash, isInteracting);
        Anim.CrossFade(targetAnim, 0.2f);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false,
        bool mirrorAnim = false) {
        Anim.applyRootMotion = isInteracting;
        Anim.SetBool(CanRotateHash, canRotate);
        Anim.SetBool(IsInteractingHash, isInteracting);
        Anim.CrossFade(targetAnim, 0.2f);
    }

    public void PlayTargetAnimationWithRootRotation(int targetAnim, bool isInteracting) {
        Anim.applyRootMotion = isInteracting;
        Anim.SetBool(IsRotatingWithRMHash, true);
        Anim.SetBool(IsInteractingHash, isInteracting);
        Anim.CrossFade(targetAnim, 0.2f);
    }

    //reposition character model when using root motion
    protected virtual void OnAnimatorMove() {
        if (characterManager.IsInteracting == false) return;

        Vector3 deltaPosition = Anim.deltaPosition;
        characterManager.CharacterController.Move(deltaPosition);
        characterManager.transform.rotation *= Anim.deltaRotation;

        // if (aiCharacter.isRotatingWithRootMotion)
        // aiCharacter.transform.rotation *= Anim.deltaRotation;
    }

    //TODO: ALL EVENTS SHOULD BE HERE 

    public static event Action ComboDisabled;
    public virtual void EnableRotation() => Anim.SetBool(CanRotateHash, true);
    public virtual void DisableRotation() => Anim.SetBool(CanRotateHash, false);

    public virtual void EnableCombo() => Anim.SetBool(CanDoComboHash, true);

    public virtual void DisableCombo() {
        Anim.SetBool(CanDoComboHash, false);
        ComboDisabled?.Invoke();
    }

    public virtual void EnableIsInvulnerable() => Anim.SetBool(IsInvulnerableHash, true);
    public virtual void DisableIsInvulnerable() => Anim.SetBool(IsInvulnerableHash, false);

    //TODO: does this actually work?
    public virtual void EnableIsParrying() => characterManager.IsParrying = true;
    public virtual void DisableIsParrying() => characterManager.IsParrying = false;
    public virtual void EnableCanBeRiposted() => characterManager.CanBeRiposted = true;
    public virtual void DisableCanBeRiposted() => characterManager.CanBeRiposted = false;
    public virtual void TakeCriticalDamageAnimEvent() { }
}
}