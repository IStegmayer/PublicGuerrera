using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna
{
    public class CharacterInventoryManager : MonoBehaviour {
        public Item CurrentItemBeingUsed;
        public SpellItem CurrentSpell;
        public WeaponItem RightWeapon;
        public WeaponItem LeftWeapon;
        public WeaponItem UnarmedWeapon;

        public WeaponItem[] WeaponsInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] WeaponsInLeftHandSlots = new WeaponItem[1];
        public int CurrentRightWeaponIndex = 0;
        public int CurrentLeftWeaponIndex = 0;

        public List<WeaponItem> WeaponsInventory = new List<WeaponItem>();

        private CharacterWeaponSlotManager characterWeaponSlotManager;

        private void Awake() {
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        }

        private void Start() {
            LeftWeapon = WeaponsInLeftHandSlots[0];
            RightWeapon = WeaponsInRightHandSlots[0];
            characterWeaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);
            characterWeaponSlotManager.LoadWeaponOnSlot(RightWeapon, false);
        }

        public void ChangeRightWeapon() {
            CurrentRightWeaponIndex++;
            if (CurrentRightWeaponIndex < WeaponsInRightHandSlots.Length &&
                WeaponsInRightHandSlots[CurrentRightWeaponIndex] != null) {
                LoadRightWeapon(WeaponsInRightHandSlots[CurrentRightWeaponIndex]);
            }
            else {
                LoadRightWeapon(UnarmedWeapon);
                CurrentRightWeaponIndex = -1;
            }
        }

        private void LoadRightWeapon(WeaponItem _weapon) {
            RightWeapon = _weapon;
            characterWeaponSlotManager.LoadWeaponOnSlot(_weapon, false);
        }
        
        public void ChangeLeftWeapon() {
            CurrentLeftWeaponIndex++;
            if (CurrentLeftWeaponIndex < WeaponsInLeftHandSlots.Length &&
                WeaponsInLeftHandSlots[CurrentLeftWeaponIndex] != null) {
                LoadLeftWeapon(WeaponsInLeftHandSlots[CurrentLeftWeaponIndex]);
            }
            else {
                LoadLeftWeapon(UnarmedWeapon);
                CurrentLeftWeaponIndex = -1;
            }
        }
        private void LoadLeftWeapon(WeaponItem _weapon) {
            LeftWeapon = _weapon;
            characterWeaponSlotManager.LoadWeaponOnSlot(_weapon, true);
        }
    }
}