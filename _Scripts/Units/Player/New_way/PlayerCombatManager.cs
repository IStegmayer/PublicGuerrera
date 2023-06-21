using System;
using UnityEngine;

namespace Dzajna {
// TODO: All this hash management is very inefficient, weaponItem should have its animations already hashed 
// also combo can just be an array of attacks and have a tree in anim. Overall a faulty script
public class PlayerCombatManager : CharacterCombatManager {
    private PlayerCharacterAnimatorManager playerCharacterAnimatorManager;
    private PlayerEquipmentManager playerEquipmentManager;
    private PlayerManager playerManager;
    private CharacterInventoryManager characterInventoryManager;
    private PlayerStatsManager playerStatsManager;
    private PlayerWeaponSlotManager playerWeaponSlotManager;
    private PlayerEffectsManager playerEffectsManager;
    private InputHandler inputHandler;
    private PlayerCamera playerCamera;

    public void Awake() {
        playerCharacterAnimatorManager = GetComponent<PlayerCharacterAnimatorManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerManager = GetComponent<PlayerManager>();
        characterInventoryManager = GetComponent<CharacterInventoryManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerWeaponSlotManager = GetComponentInChildren<PlayerWeaponSlotManager>();
    }

    protected override void Start() {
        base.Start();
        inputHandler = InputHandler.Instance;
        playerCamera = PlayerCamera.Instance;
    }

    public void HandleLTAction() {
        WeaponItem weapon = characterInventoryManager.LeftWeapon;
        if (weapon.WeaponType == WeaponType.Shield) {
            PerformLTWeaponArt(weapon, false);
        }
    }

    public void HandleRBAction() {
        if (!playerStatsManager.CheckHasStamina()) return;
        WeaponItem weapon = characterInventoryManager.RightWeapon;
        if (weapon.WeaponType == WeaponType.StraightSword) PerformRBMeleeAction(weapon);
        else if (weapon.WeaponType == WeaponType.MagicCaster) PerformRBMagicCast(weapon);
    }

    public void HandleLBAction() {
        PerformLBBlockingAction();
    }

    private void PerformRBMeleeAction(WeaponItem weapon) {
        if (playerManager.CanDoCombo) {
            inputHandler.SetComboFlag(true);
            HandleWeaponCombo(weapon);
            inputHandler.SetComboFlag(false);
        }
        else if (!playerManager.IsInteracting && !playerManager.CanDoCombo) {
            playerCharacterAnimatorManager.Anim.SetBool(playerCharacterAnimatorManager.IsUsingRightHandHash, true);
            HandleLightAttack(weapon);
        }
    }

    private void PerformRBMagicCast(WeaponItem weapon) {
        if (playerManager.IsInteracting) return;

        SpellItem spell = characterInventoryManager.CurrentSpell;
        if (spell != null && spell.isMagicSpell) {
            bool canCast = playerStatsManager.CheckIfCanCast(spell.FPCost);
            if (canCast)
                spell.AttemptToCastSpell(playerManager);
            else
                playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.FailedCastHash, true);
        }
    }

    private void PerformLTWeaponArt(WeaponItem weapon, bool isTwoHanding) {
        if (playerManager.IsInteracting) return;

        if (isTwoHanding) { }
        else {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.WeaponArtHash, true);
        }

        // If we are two handing perform weapon art for right weapon
        // else perform art for left handed weapon
    }

    public void HandleWeaponCombo(WeaponItem weaponItem) {
        if (!inputHandler.ComboFlag) return;

        //TODO: not good
        if (LastAttackHash == playerCharacterAnimatorManager.LightAttack1Hash) {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.LightAttack2Hash, true);
        }
        else if (LastAttackHash == playerCharacterAnimatorManager.HeavyAttack1Hash) {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.HeavyAttack2Hash, true);
        }
        else if (LastAttackHash == playerCharacterAnimatorManager.TH_LightAttack1Hash) {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.TH_LightAttack2Hash,
                true);
        }
        else if (LastAttackHash == playerCharacterAnimatorManager.TH_HeavyAttack1Hash) {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.TH_HeavyAttack2Hash,
                true);
        }

        playerCharacterAnimatorManager.Anim.SetBool("canDoCombo", false);
    }

    private void HandleLightAttack(WeaponItem weaponItem) {

        if (inputHandler.TwoHandFlag) {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.TH_LightAttack1Hash,
                true);
            LastAttackHash = playerCharacterAnimatorManager.TH_LightAttack1Hash;
        }
        else {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.LightAttack1Hash, true);
            LastAttackHash = playerCharacterAnimatorManager.LightAttack1Hash;
        }
    }

    private void PerformLBBlockingAction() {
        if (playerManager.IsInteracting) return;
        if (playerManager.IsBlocking) return;

        playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.BlockHash, false, true);
        playerManager.IsBlocking = true;
    }

    // TODO: im checking here for now
    public void HandleHeavyAttack(WeaponItem weaponItem) {
        if (!playerStatsManager.CheckHasStamina()) return;

        if (inputHandler.TwoHandFlag) {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.TH_HeavyAttack1Hash,
                true);
            LastAttackHash = playerCharacterAnimatorManager.TH_HeavyAttack1Hash;
        }
        else {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.HeavyAttack1Hash, true);
            LastAttackHash = playerCharacterAnimatorManager.HeavyAttack1Hash;
        }
    }

    //TODO: again this could be compressed...
    public void DrainStaminaLightAttack() {
        CharacterInventoryManager inventory = playerManager.CharacterInventoryManager;
        if (playerManager.IsUsingLeftHand)
            playerStatsManager.TakeStaminaDamage(inventory.LeftWeapon.BaseStamina *
                                                 inventory.LeftWeapon.LightAttackMultiplier);
        else if (playerManager.IsUsingRightHand)
            playerStatsManager.TakeStaminaDamage(inventory.RightWeapon.BaseStamina *
                                                 inventory.RightWeapon.LightAttackMultiplier);
    }

    public void DrainStaminaHeavyAttack() {
        CharacterInventoryManager inventory = playerManager.CharacterInventoryManager;
        if (playerManager.IsUsingLeftHand)
            playerStatsManager.TakeStaminaDamage(inventory.LeftWeapon.BaseStamina *
                                                 inventory.LeftWeapon.HeavyAttackMultiplier);
        else if (playerManager.IsUsingRightHand)
            playerStatsManager.TakeStaminaDamage(inventory.RightWeapon.BaseStamina *
                                                 inventory.RightWeapon.HeavyAttackMultiplier);
    }
}
}