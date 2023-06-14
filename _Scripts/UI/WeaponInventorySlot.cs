using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dzajna
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private WeaponItem item;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private EquipmentWindowUI equipmentWindowUI;

        private CharacterInventoryManager characterInventoryManager;
        private PlayerWeaponSlotManager playerWeaponSlotManager;

        public void AddItem(WeaponItem _newItem) {
            item = _newItem;
            icon.sprite = item.ItemIcon;
            gameObject.SetActive(true);
            icon.enabled = true;
        }

        public void ClearInventorySlot() {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem() {
            //TODO: this is getting ridiculous with all these player references that cant be set on awake...
            if (characterInventoryManager == null) characterInventoryManager = FindObjectOfType<CharacterInventoryManager>();
            if (playerWeaponSlotManager == null) playerWeaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();

            if (uiManager.RightHandSlot1Selected) {
                characterInventoryManager.WeaponsInventory.Add(characterInventoryManager.WeaponsInRightHandSlots[0]);
                characterInventoryManager.WeaponsInRightHandSlots[0] = item;
                characterInventoryManager.WeaponsInventory.Remove(item);
            }
            else if (uiManager.RightHandSlot2Selected) {
                characterInventoryManager.WeaponsInventory.Add(characterInventoryManager.WeaponsInRightHandSlots[1]);
                characterInventoryManager.WeaponsInRightHandSlots[1] = item;
                characterInventoryManager.WeaponsInventory.Remove(item);
            }
            else if (uiManager.LeftHandSlot1Selected) {
                characterInventoryManager.WeaponsInventory.Add(characterInventoryManager.WeaponsInLeftHandSlots[0]);
                characterInventoryManager.WeaponsInLeftHandSlots[0] = item;
                characterInventoryManager.WeaponsInventory.Remove(item);
            }
            else if (uiManager.LeftHandSlot2Selected) {
                characterInventoryManager.WeaponsInventory.Add(characterInventoryManager.WeaponsInLeftHandSlots[1]);
                characterInventoryManager.WeaponsInLeftHandSlots[1] = item;
                characterInventoryManager.WeaponsInventory.Remove(item);
            }
            else return;

            characterInventoryManager.RightWeapon =
                characterInventoryManager.WeaponsInRightHandSlots[characterInventoryManager.CurrentRightWeaponIndex];
            characterInventoryManager.LeftWeapon = characterInventoryManager.WeaponsInLeftHandSlots[characterInventoryManager.CurrentLeftWeaponIndex];
            
            playerWeaponSlotManager.LoadWeaponOnSlot(characterInventoryManager.RightWeapon, false);
            playerWeaponSlotManager.LoadWeaponOnSlot(characterInventoryManager.LeftWeapon, true);
            
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(characterInventoryManager);
            uiManager.ResetAllSelectedSlots();
        }
    }
}