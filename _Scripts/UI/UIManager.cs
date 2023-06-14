using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CharacterInventoryManager characterInventoryManager;
        
        [Header("UI Windows")]
        [SerializeField] private GameObject HUDWindow;
        [SerializeField] private GameObject selectWindow;
        [SerializeField] private GameObject weaponInventoryWindow;
        [SerializeField] private GameObject equipmentWindow;

        [Header("Equipment Window Slot Selected")]
        public bool RightHandSlot1Selected;
        public bool RightHandSlot2Selected;
        public bool LeftHandSlot1Selected;
        public bool LeftHandSlot2Selected;

        [Header("Weapons Inventory")]
        [SerializeField] private Transform weaponInventorySlotsContainer;
        [SerializeField] private Transform weaponInventorySlotPrefab;
        [SerializeField] private EquipmentWindowUI equipmentWindowUI;
        private WeaponInventorySlot[] weaponInventorySlots;

        private void Awake() {
            equipmentWindowUI = GetComponentInChildren<EquipmentWindowUI>(true);
        }

        private void Start() {
            weaponInventorySlots = weaponInventorySlotsContainer.GetComponentsInChildren<WeaponInventorySlot>(true);
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(characterInventoryManager);
        }

        public void UpdateUI() {
            #region WeaponInventorySlots

            int inventoryCount = characterInventoryManager.WeaponsInventory.Count;
            for (int i = 0; i < weaponInventorySlots.Length; i++) {
                if (i < inventoryCount) {
                    if (weaponInventorySlots.Length < inventoryCount) {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsContainer);
                        weaponInventorySlots =
                            weaponInventorySlotsContainer.GetComponentsInChildren<WeaponInventorySlot>(true);
                    }

                    weaponInventorySlots[i].AddItem(characterInventoryManager.WeaponsInventory[i]);
                }
                else {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }

            #endregion
        }

        public void ToggleSelectWindow(bool active) => selectWindow.SetActive(active);
        public void ToggleHUDWindow(bool active) => HUDWindow.SetActive(active);
        public void CloseAllInventoryWindows() {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentWindow.SetActive(false);
        }
        
        public void ResetAllSelectedSlots() {
            RightHandSlot1Selected = false;
            RightHandSlot2Selected = false;
            LeftHandSlot1Selected = false;
            LeftHandSlot2Selected = false;
        }
    }
}