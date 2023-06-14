using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dzajna
{
    public class HealthBar : MonoBehaviour
    {
        private Slider slider;

        private void Awake() {
            slider = GetComponent<Slider>();
        }

        public void SetMaxHealth(float _maxHealth) {
            slider.maxValue = _maxHealth;
            slider.value = _maxHealth;
        }

        public void SetCurrentHealth(float _currentHealth) {
            slider.value = _currentHealth;
        }
    }
}