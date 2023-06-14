using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dzajna
{
    public class EquipmentSlotUI : MonoBehaviour
    {
        public WeaponItem weaponItem;
        [SerializeField] private Image icon;
        [SerializeField] private UIManager uiManager;

        public bool RightHandSlot1;
        public bool RightHandSlot2;
        public bool LeftHandSlot1;
        public bool LeftHandSlot2;

        public void AddItem(WeaponItem _weaponItem) {
            weaponItem = _weaponItem;
            icon.sprite = _weaponItem.ItemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem() {
            weaponItem = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void SelectThisSlot() {
            if (RightHandSlot1) uiManager.RightHandSlot1Selected = true;
            else if (RightHandSlot2) uiManager.RightHandSlot2Selected = true;
            else if (LeftHandSlot1) uiManager.LeftHandSlot1Selected = true;
            else uiManager.LeftHandSlot2Selected = true;
        }
    }
}