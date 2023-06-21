using System;
using UnityEngine;

namespace Dzajna
{
    public class CharacterManager : MonoBehaviour {
        public Animator animator;
        public CharacterAnimatorManager CharacterAnimatorManager;
        public CharacterWeaponSlotManager CharacterWeaponSlotManager;
        public CharacterStatsManager CharacterStatsManager;
        public CharacterInventoryManager CharacterInventoryManager;
        public CharacterEffectsManager CharacterEffectsManager;
        public CharacterCombatManager CharacterCombatManager;
        // public CharacterSoundFXManager characterSoundFXManager;
        public CharacterController CharacterController;
        
        [Header("Lock On Transform")]
        public Transform LockOnTransform;

        [Header("Combat Flags")] 
        public bool CanBeRiposted;
        public bool CanBeParried;
        public bool CanDoCombo;
        public bool IsParrying;
        public bool IsBlocking;
        public bool IsInvulnerable;
        
        [Header("Player flags")] 
        public bool IsInteracting;
        public bool IsDead;
        public bool IsAttacking;
        public bool IsSprinting;
        public bool IsGrounded = true;
        public bool IsUsingRightHand;
        public bool IsUsingLeftHand;
        public bool IsTwoHandingWeapon;

        [Header("Movement Flags")] 
        public bool IsRotatingWithRM;
        
        [Header("Spell Flags")]
        public bool IsFiringSpell;

        // Damage will be inflicted during an animation event
        // Used in backstab or riposte animations
        public float PendingCriticalDamage;
        public bool IsBeingBackstabbed { get; set; }
        public bool IsBeingRiposted { get; set; }
        public bool IsPerformingBackstab { get; set; }
        public bool IsPerformingRiposte { get; set; }

        protected virtual void Awake() {
            CharacterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            CharacterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            CharacterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            CharacterStatsManager = GetComponent<CharacterStatsManager>();
            CharacterInventoryManager = GetComponent<CharacterInventoryManager>();
            CharacterEffectsManager = GetComponent<CharacterEffectsManager>();
            CharacterCombatManager = GetComponent<CharacterCombatManager>();
            // characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        }

        protected virtual void Start() {
        }
        
        public void BreakGuard() {
            CharacterAnimatorManager.PlayTargetAnimation(CharacterAnimatorManager.ParriedHash, true);
        }
        
        public virtual void UpdateWhichHandCharacterIsUsing(bool usingRightHand) {
            if (usingRightHand) {
                IsUsingRightHand = true;
                IsUsingLeftHand = false;
            }
            else {
                IsUsingLeftHand = true;
                IsUsingRightHand = false;
            }
        }
    }
}