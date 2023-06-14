using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Dzajna {
public class PlayerWeaponSlotManager : CharacterWeaponSlotManager {
    private PlayerManager playerManager;
    private PlayerCharacterAnimatorManager animManager;
    private QuickSlotsUI quickSlotsUI;
    private PlayerStatsManager playerStatsManager;

    private void Awake() {
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        animManager = GetComponentInParent<PlayerCharacterAnimatorManager>();
        //TODO: this could just be a serialized field
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        
        LoadWeaponHolderSlots();
    }

    public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft) {
        WeaponItem weaponToEquip = weaponItem != null ? weaponItem : unarmedWeapon;
        
        if (isLeft) {
            leftHandSlot.CurrentWeaponItem = weaponToEquip;
            leftHandSlot.LoadWeaponModel(weaponToEquip);
            LoadLeftWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponToEquip, true);
            animManager.PlayTargetAnimation(animManager.LeftArmIdleHash, false);
        }
        else {
            if (InputHandler.Instance.TwoHandFlag) {
                backSlot.LoadWeaponModel(leftHandSlot.CurrentWeaponItem);
                animManager.PlayTargetAnimation(animManager.TH_IdleHash, false);
                leftHandSlot.UnloadWeaponAndDestroy();
            }
            else {
                animManager.PlayTargetAnimation(animManager.RightArmIdleHash,false);
                backSlot.UnloadWeaponAndDestroy();
            }

            rightHandSlot.CurrentWeaponItem = weaponToEquip;
            rightHandSlot.LoadWeaponModel(weaponToEquip);
            LoadRightWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponToEquip, false);
            animManager.Anim.runtimeAnimatorController = weaponItem.WeaponController;
        }
    }
    
}
}