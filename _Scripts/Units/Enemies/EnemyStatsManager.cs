using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {
// TODO: I'll get back to this when I do AI 
public class EnemyStatsManager : CharacterStatsManager {
    [SerializeField] private UIEnemyHealthBar healthBar;
    public int SoulsReward;
    public bool IsBoss;
    private EnemyManager enemyManager;
    private EnemyCharacterAnimatorManager enemyCharacterAnimatorManager;
    private EnemyBossManager enemyBossManager;

    private Coroutine deathRoutine;

    private void Awake() {
        enemyManager = GetComponent<EnemyManager>();
        enemyCharacterAnimatorManager = GetComponent<EnemyCharacterAnimatorManager>();
        enemyBossManager = GetComponent<EnemyBossManager>();
        maxHealth = SetMaxHealthFromHealthLevel(); 
        currentHealth = maxHealth;
    }

    private void Start() {
        if (!IsBoss) healthBar.SetMaxHealth(maxHealth);
    }

    protected override void HandlePoiseResetTimer() {
        if (PoiseResetTimer > 0) PoiseResetTimer = PoiseResetTimer - Time.deltaTime;
        else if (!enemyManager.IsInteracting) TotalPoise = ArmorPoise;
    }
    
    private int SetMaxHealthFromHealthLevel() {
        return healthLevel * healthPerLevel;
    }

    public override void TakeDamageWithoutAnimation(float dmg) {
        if (enemyManager.IsDead) return;
        if (enemyManager.IsInvulnerable && !enemyManager.IsBeingRiposted) return;
        
        currentHealth -= dmg;
        if (IsBoss && enemyBossManager != null) enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        else if (healthBar != null) healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0) {
            // enemyCharacterAnimatorManager.PlayTargetAnimation("Death", true);
            deathRoutine = StartCoroutine(DeathRoutine());
            enemyManager.IsDead = true;
        }
    }

    public override void TakeDamage(float dmg, string dmgAnimation = "Damage1") {
        if (enemyManager.IsDead || enemyManager.IsInvulnerable) return;
        
        currentHealth -= dmg;
        if (IsBoss && enemyBossManager != null) {
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }
        else if (healthBar != null) healthBar.SetCurrentHealth(currentHealth);

        if (enemyManager.IsPhaseShifting) return;
        

        if (currentHealth <= 0) {
            enemyCharacterAnimatorManager.PlayTargetAnimation("Death", true);
            deathRoutine = StartCoroutine(DeathRoutine());
            enemyManager.IsDead = true;
        }
        else {
            enemyCharacterAnimatorManager.PlayTargetAnimation(dmgAnimation, true);
        }
    }

    private IEnumerator DeathRoutine() {
        yield return new WaitForSeconds(1f);
        //TODO: This could be a bitch. 
        GameManager.Instance.ChangeState(GameState.Win);
        yield return new WaitForSeconds(5f);
        UnitManager.Instance.DespawnEnemy(transform.gameObject);
    }

    public float GetMaxHealth() => maxHealth;
}
}