using System;
using UnityEngine;

namespace Dzajna {
public class CharacterStatsManager : MonoBehaviour {
    //TODO: ofc this is all simplified
    [SerializeField] protected int healthLevel = 10;
    [SerializeField] protected int healthPerLevel = 5;
    [SerializeField] protected int staminaLevel = 10;
    [SerializeField] protected int staminaPerLevel = 5;
    [SerializeField] protected float staminaRegenAmount = 2f;
    [SerializeField] protected int focusLevel = 10;
    [SerializeField] protected int focusPerLevel = 5;

    [Header("Poise")] //
    public float TotalPoise;
    public float OffensivePoise;
    public float ArmorPoise;
    public float TotalPoiseResetTime = 5;
    public float PoiseResetTimer = 0;
    
    [Header("Blocking Absorptions")] //
    public float BlockingPhysicalDamageAbsorption;
    public float BlockingFireDamageAbsorption;
    public float BlockingStabilityRating;

    protected float maxHealth;
    protected float currentHealth;
    protected float maxStamina;
    protected float currentStamina;
    protected float maxFP;
    protected float currentFP;
    protected float staminaRegenTimer;

    public virtual void TakeDamage(float dmg, string dmgAnimation = "Damage1") { }
    public virtual void TakeDamageWithoutAnimation(float dmg) { }

    protected void Update() {
        HandlePoiseResetTimer();
    }

    private void Start() {
        TotalPoise = ArmorPoise;
    }
    
    public virtual void TakeStaminaDamage(float dmg) {
        currentStamina -= dmg;
    }

    protected virtual void HandlePoiseResetTimer() { }
    public bool CheckHasStamina() => currentStamina >= 0;
    public bool CheckHasEnoughFP(float cost) => currentFP >= cost;
    public void ResetPoiseTimer() => PoiseResetTimer = TotalPoiseResetTime;
    public void DealPoiseDamage(float dmg) => TotalPoise = TotalPoise - dmg;
}
}