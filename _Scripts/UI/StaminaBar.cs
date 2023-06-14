using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dzajna
{
    public class StaminaBar : MonoBehaviour
    {
        private Slider slider;

        private void Awake() {
            slider = GetComponent<Slider>();
        }

        public void SetMaxStamina(float _maxStamina) {
            slider.maxValue = _maxStamina;
            slider.value = _maxStamina;
        }

        public void SetCurrentStamina(float _currentStamina) {
            slider.value = _currentStamina;
        }
    }
}