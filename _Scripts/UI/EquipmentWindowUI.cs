using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public bool RightHandSlot1Selected;
        public bool RightHandSlot2Selected;
        public bool LeftHandSlot1Selected;
        public bool LeftHandSlot2Selected;

        private EquipmentSlotUI[] equipmentSlotUis;

        public void LoadWeaponsOnEquipmentScreen(CharacterInventoryManager characterInventoryManager) {
            equipmentSlotUis = GetComponentsInChildren<EquipmentSlotUI>(true);
            for (int _i = 0; _i < equipmentSlotUis.Length; _i++) {
                if (equipmentSlotUis[_i].RightHandSlot1)
                    equipmentSlotUis[0].AddItem(characterInventoryManager.WeaponsInRightHandSlots[0]);
                else if (equipmentSlotUis[_i].RightHandSlot2)
                    equipmentSlotUis[1].AddItem(characterInventoryManager.WeaponsInRightHandSlots[1]);
                else if (equipmentSlotUis[_i].LeftHandSlot1)
                    equipmentSlotUis[2].AddItem(characterInventoryManager.WeaponsInLeftHandSlots[0]);
                else if (equipmentSlotUis[_i].LeftHandSlot2)
                    equipmentSlotUis[3].AddItem(characterInventoryManager.WeaponsInLeftHandSlots[1]);
            }
        }

        public void SelectRightHandSlot1() => RightHandSlot1Selected = true;
        public void SelectRightHandSlot2() => RightHandSlot2Selected = true;
        public void SelectLeftHandSlot1() => LeftHandSlot1Selected = true;
        public void SelectLeftHandSlot2() => LeftHandSlot2Selected = true;
    }
}