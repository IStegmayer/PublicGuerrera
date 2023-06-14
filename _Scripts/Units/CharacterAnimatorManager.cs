using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dzajna {
public class CharacterAnimatorManager : MonoBehaviour {
    public CharacterManager characterManager;
    public Animator Anim;
    public bool CanRotate;

    // properties hashes
    public int IsGroundedHash;
    public int IsUsingRightHandHash;
    public int IsUsingLeftHandHash;
    public int VerticalHash;
    public int HorizontalHash;
    public int IsInteractingHash;
    public int IsDeadHash;
    public int IsBlockingHash;
    public int CanRotateHash;
    public int IsFiringSpellHash;
    public int IsRotatingWithRMHash;
    public int IsPhaseShiftingHash;
    public int CanDoComboHash;
    public int IsInvulnerableHash;
    public int InAirTimer;
    public int IsChargingAttackHash;
    public int IsTwoHandingWeaponHash;

    // states hashes
    public int RollHash;
    public int BackstepHash;
    public int LandHash;
    public int FallHash;
    public int EmptyHash;
    public int LightAttack1Hash;
    public int LightAttack2Hash;
    public int HeavyAttack1Hash;
    public int HeavyAttack2Hash;
    public int RunningAttackHash;
    public int TH_LightAttack1Hash;
    public int TH_LightAttack2Hash;
    public int TH_HeavyAttack1Hash;
    public int TH_HeavyAttack2Hash;
    public int TH_RunningAttackHash;
    public int WeaponArtHash;
    public int LeftArmIdleHash;
    public int RightArmIdleHash;
    public int TH_IdleHash;
    public int DeathHash;
    public int JumpHash;
    public int FailedCastHash;
    public int BackStabHash;
    public int RiposteHash;
    public int RipostedHash;
    public int BackStabbedHash;
    public int OpenChestHash;
    public int PassThroughFogWallHash;
    public int BlockHash;
    public int GuardBreakHash;
    public int BlockDamage1Hash;
    public int Damage1Hash;
    public int TurnAroundHash;
    public int TurnRightHash;
    public int TurnLeftHash;
    public int PhaseTransitionHash;
    public int ParriedHash;
    public int ParryHash;

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