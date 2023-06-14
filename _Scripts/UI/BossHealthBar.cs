using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dzajna {
public class BossHealthBar : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI bossName;
    private Slider slider;

    private void Awake() {
        slider = GetComponentInChildren<Slider>(true);
    }

    private void Start() {
        ToggleBossHealthBarActivation(false);
    }

    public void SetBossName(string _name) => bossName.text = _name;
    public void ToggleBossHealthBarActivation(bool _val) => gameObject.SetActive(_val);
    public void SetBossCurrentHealth(float _currentHealth) => slider.value = _currentHealth;

    public void SetBossMaxHealth(float _maxHealth) {
        slider.maxValue = _maxHealth;
        slider.value = _maxHealth;
    }
}
}