using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {
public class PlayerStatsManager : CharacterStatsManager {
    private PlayerManager player;
    private PlayerCharacterAnimatorManager playerCharacterAnimatorManager;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private StaminaBar staminaBar;
    [SerializeField] private StatusBar FPBar;
    private Coroutine deathRoutine;

    public int SoulCount = 0;

    private void Awake() {
        player = GetComponent<PlayerManager>();
        playerCharacterAnimatorManager = GetComponent<PlayerCharacterAnimatorManager>();
    }

    private void Start() {
        maxHealth = SetMaxHealthFromHealthLevel();
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;

        maxStamina = SetMaxStaminaFromStaminaLevel();
        // set stamina max bar
        staminaBar.SetMaxStamina(maxStamina);
        currentStamina = maxStamina;

        maxFP = SetMaxFPAmountFromFocusLevel();
        FPBar.SetMaxStat(maxFP);
        currentFP = maxFP;
    }

    protected override void HandlePoiseResetTimer() {
        if (PoiseResetTimer > 0) PoiseResetTimer = PoiseResetTimer - Time.deltaTime;
        else if (!player.IsInteracting) TotalPoise = ArmorPoise;
    }

    private int SetMaxHealthFromHealthLevel() {
        return healthLevel * healthPerLevel;
    }

    private int SetMaxStaminaFromStaminaLevel() {
        return staminaLevel * staminaPerLevel;
    }
    private int SetMaxFPAmountFromFocusLevel() {
        return focusLevel * focusPerLevel;
    }

    public bool CheckIfCanCast(int FPCost) {
        return currentFP >= FPCost;
    }

    public override void TakeDamageWithoutAnimation(float dmg) {
        if (player.IsDead || player.IsInvulnerable) return;
        currentHealth -= dmg;
            
        if (currentHealth <= 0) {
            deathRoutine = StartCoroutine(DeathRoutine());
            player.IsDead = true;
        }
    }

    public override void TakeDamage(float dmg, string dmgAnimation = "Damage1") {
        if (player.IsDead || player.IsInvulnerable) return;
        
        currentHealth -= dmg;
        
        if (currentHealth <= 0) {
            playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.DeathHash, true);   
            deathRoutine = StartCoroutine(DeathRoutine());
            player.IsDead = true;
        }
        
        healthBar.SetCurrentHealth(currentHealth);
        playerCharacterAnimatorManager.PlayTargetAnimation(dmgAnimation, true);
    }
    public override void TakeStaminaDamage(float dmg) {
        base.TakeStaminaDamage(dmg);
        staminaBar.SetCurrentStamina(currentStamina);
    }

    private IEnumerator DeathRoutine() {
        // start death animation
        yield return new WaitForSeconds(1f);
        GameManager.Instance.ChangeState(GameState.Lose);
        yield return new WaitForSeconds(1f);
        UnitManager.Instance.DespawnPlayer(transform.gameObject);
    }

    public void TakeFPDamage(float dmg) {
        currentFP -= dmg;
        FPBar.SetCurrentStat(currentFP);
    }

    public void RegenerateStamina() {
        if (currentStamina >= maxStamina) return;

        if (player.IsInteracting || player.IsSprinting) staminaRegenTimer = 0;
        else {
            staminaRegenTimer += Time.deltaTime;
            if (staminaRegenTimer > 1f) {
                currentStamina += staminaRegenAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(currentStamina);
            }
        }
    }

    public void AddSouls(int souls) => SoulCount += souls;
}
}