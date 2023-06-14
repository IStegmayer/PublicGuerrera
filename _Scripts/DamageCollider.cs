using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.TextCore.Text;

namespace Dzajna {
public class DamageCollider : MonoBehaviour {
    public CharacterManager characterManager;
    public bool EnabledOnStartup;
    protected Collider damageCollider;

    [Header("Damage")] //
    public float PhysicalDamage = 25;

    public float FireDamage;

    [Header("Poise")] //
    public float PoiseBreak = 10;

    public float OffensivePoiseBonus = 10;

    [Header("Guard Break Modifier")] //
    public float GuardBreakModifier = 1;

    private readonly List<CharacterManager> charactersDamagedDuringThisCalculation = new List<CharacterManager>();
    protected string currentDamageAnimation;
    protected bool hasBeenParried;
    protected bool shieldHasBeenHit;

    private void Awake() {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = EnabledOnStartup;
    }

    public void EnableDamageCollider() => damageCollider.enabled = true;

    public void DisableDamageCollider() {
        damageCollider.enabled = false;
        charactersDamagedDuringThisCalculation.Clear();
    }


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DamageableCharacter")) {
            shieldHasBeenHit = false;
            hasBeenParried = false;

            CharacterManager enemyManager = collision.gameObject.GetComponentInParent<CharacterManager>();
            if (enemyManager == null) throw new Exception("No enemy character manager found.");

            // TODO: check for teamID to not hit
            if (enemyManager == characterManager) return;
            if (charactersDamagedDuringThisCalculation.Contains(enemyManager)) return;
            charactersDamagedDuringThisCalculation.Add(enemyManager);

            CheckForBlock(enemyManager);
            CheckForParry(enemyManager);
            if (shieldHasBeenHit || hasBeenParried) return;

            CharacterEffectsManager charEffectsManager = enemyManager.CharacterEffectsManager;
            CharacterStatsManager charStats = enemyManager.CharacterStatsManager;
            string dmgAnim = enemyManager.IsBlocking ? "BlockDamage1" : "Damage1";
            Vector3 contactPoint = collision.ClosestPointOnBounds(transform.position);

            charEffectsManager.PlayBloodSplatterFX(contactPoint);
            charStats.ResetPoiseTimer();
            charStats.DealPoiseDamage(PoiseBreak);
            if (charStats.TotalPoise > PoiseBreak) charStats.TakeDamage(PhysicalDamage, dmgAnim);
            else {
                charStats.TakeDamageWithoutAnimation(PhysicalDamage);
                enemyManager.BreakGuard();
            }

            var aiCharacter = enemyManager as EnemyManager;
            if (aiCharacter != null && characterManager.GetType() != typeof(EnemyManager))
                aiCharacter.CurrentTarget = characterManager;
        }
    }

    protected virtual void CheckForParry(CharacterManager enemyManager) {
        if (!enemyManager.IsParrying) return;

        var charAnim = characterManager.CharacterAnimatorManager;
        charAnim.PlayTargetAnimation(charAnim.ParriedHash, true);
        hasBeenParried = true;
    }

    protected virtual void CheckForBlock(CharacterManager enemyManager) {
        if (!enemyManager.IsBlocking) return;

        // Check if enemy is hitting from the front
        var enemyShield = enemyManager.CharacterStatsManager;
        var directionFromPlayerToEnemy = characterManager.transform.position - enemyManager.transform.position;
        var dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);
        if (dotValueFromPlayerToEnemy < 0.3f) return;

        shieldHasBeenHit = true;
        var physicalDamageAfterBlock =
            PhysicalDamage - PhysicalDamage * enemyShield.BlockingPhysicalDamageAbsorption / 100;

        enemyManager.CharacterCombatManager.AttemptBlock(this, PhysicalDamage, FireDamage, "Block_01");
        enemyShield.TakeDamage(physicalDamageAfterBlock);
    }
}
}