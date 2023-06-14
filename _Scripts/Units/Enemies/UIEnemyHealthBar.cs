using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dzajna {
public class UIEnemyHealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;
    private float timeUntilBarIsHidden;

    // private void Awake() {
    //     slider = GetComponentInChildren<Slider>(true);
    // }

    public void SetCurrentHealth(float _health) {
        slider.value = _health;
        timeUntilBarIsHidden = 3f;
        if (!slider.gameObject.activeInHierarchy) slider.gameObject.SetActive(true);
    }

    public void SetMaxHealth(float _maxHealth) {
        slider.maxValue = _maxHealth;
        slider.value = _maxHealth;
    }

    private void Update() {
        if (slider == null) return;
        
        if (timeUntilBarIsHidden <= 0) {
            timeUntilBarIsHidden = 0;
            slider.gameObject.SetActive(false);
        }
        else timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;
        
        
        if (slider.value <= 0) Destroy(slider.gameObject);
    }
}
}