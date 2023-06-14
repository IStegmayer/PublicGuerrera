using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dzajna
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image LeftWeaponIcon;
        public Image RightWeaponIcon;

        public void UpdateWeaponQuickSlotsUI(WeaponItem _weapon, bool _isLeft) {
            if (!_isLeft) {
                if (_weapon.ItemIcon != null) {
                    RightWeaponIcon.sprite = _weapon.ItemIcon;
                    RightWeaponIcon.enabled = true;
                }
                else {
                    RightWeaponIcon.sprite = null;
                    RightWeaponIcon.enabled = false;
                }
            }
            else {
                if (_weapon.ItemIcon != null) {
                    LeftWeaponIcon.sprite = _weapon.ItemIcon;
                    LeftWeaponIcon.enabled = true;
                }
                else {
                    LeftWeaponIcon.sprite = null;
                    LeftWeaponIcon.enabled = false;
                }
            }
        }
    }
}