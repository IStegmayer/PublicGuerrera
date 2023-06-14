using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {
public class EnemyWeaponSlotManager : CharacterWeaponSlotManager {
    private EnemyStatsManager enemyStatsManager;
    private CharacterEffectsManager characterEffectsManager;

    protected override void Awake() {
        base.Awake();
        enemyStatsManager = GetComponentInParent<EnemyStatsManager>();
        characterEffectsManager = GetComponentInParent<CharacterEffectsManager>();
        LoadWeaponHolderSlots();
    }

    protected override void Start() {
        base.Start();
        if (rightHandWeapon != null) LoadWeaponOnSlot(rightHandWeapon, false);
        if (leftHandWeapon != null) LoadWeaponOnSlot(leftHandWeapon, true);
    }

    private void LoadWeaponHolderSlots() {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots) {
            if (weaponSlot.isLeftHandSlot) leftHandSlot = weaponSlot;
            else if (weaponSlot.isRightHandSlot) rightHandSlot = weaponSlot;
        }
    }

    public override void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft) {
        WeaponItem weaponToEquip = weapon != null ? weapon : unarmedWeapon;

        if (isLeft) {
            leftHandSlot.CurrentWeaponItem = weaponToEquip;
            leftHandSlot.LoadWeaponModel(weaponToEquip);
            LoadLeftWeaponDamageCollider();
        }
        else {
            rightHandSlot.CurrentWeaponItem = weaponToEquip;
            rightHandSlot.LoadWeaponModel(weaponToEquip);
            LoadRightWeaponDamageCollider();
        }
    }

    private void LoadWeaponsDamageCollider(bool isLeft) {
        if (isLeft) {
            LeftDamageCollider = leftHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
            LeftDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
        }
        else {
            RightDamageCollider = rightHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
            RightDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
        }
    }

    //TODO: Implement these
    public void DrainStaminaLightAttack() { }
    public void DrainStaminaHeavyAttack() { }
    public void EnableCombo() { }
    public void DisableCombo() { }


    #region Handle Weapon's Damage Collider

    protected override void LoadLeftWeaponDamageCollider() {
        LeftDamageCollider =
            leftHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
        LeftDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

        characterEffectsManager.LeftWeaponFX = leftHandSlot.CurrentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    protected override void LoadRightWeaponDamageCollider() {
        RightDamageCollider =
            rightHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
        RightDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            
        characterEffectsManager.RightWeaponFX = rightHandSlot.CurrentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    public void OpenDamageCollider() {
        RightDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider() {
        RightDamageCollider.DisableDamageCollider();
    }

    //TODO: test if this actually works
    public void GrantWeaponAttackingPoiseBonus() {
        enemyStatsManager.TotalPoise += enemyStatsManager.OffensivePoise;
    }

    public void RemoveWeaponAttackingPoiseBonus() {
        enemyStatsManager.TotalPoise -= enemyStatsManager.OffensivePoise;
    }

    #endregion
}
}