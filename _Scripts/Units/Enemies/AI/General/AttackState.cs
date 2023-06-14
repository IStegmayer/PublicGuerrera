using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dzajna {
public class AttackState : State {
    [SerializeField] private CombatStanceState combatStanceState;
    [SerializeField] private PursueTargetState pursueTargetState;
    public EnemyAttackAction CurrentAttack;

    private bool willComboNextAttack;
    private bool hasPerformedAttack;

    public override State Tick(EnemyManager enemy) {
        if (enemy.IsInteracting) return this;
        
        CharacterAnimatorManager enemyAnim = enemy.CharacterAnimatorManager;

        RotateTowardsTargetWhilstAttacking(enemy, enemy.TargetsDirection);

        if (enemy.DistanceFromTarget > enemy.MaxAggroRadius) return pursueTargetState;

        if (willComboNextAttack && enemy.CanDoCombo) {
            AttackTargetWithCombo(enemy, enemyAnim);
            // TODO: this is just to finish AI refactor
            // enemy.CurrentRecoveryTime = CurrentAttack.RecoveryTime;
        }

        if (!hasPerformedAttack) {
            AttackTarget(enemy, enemyAnim);
            RollForComboChance(enemy);
        }

        if (willComboNextAttack && hasPerformedAttack) {
            return this; // Go back up to perform combo
        }

        return combatStanceState;
    }

    private void AttackTarget(EnemyManager enemyManager, CharacterAnimatorManager enemyAnim) {
        enemyAnim.PlayTargetAnimation(CurrentAttack.ActionAnimation, true);
        // TODO: this is just to finish AI refactor
        // enemyManager.CurrentRecoveryTime = CurrentAttack.RecoveryTime; // if comboing this will be ignored
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(EnemyManager enemyManager, CharacterAnimatorManager enemyAnim) {
        willComboNextAttack = false;
        enemyAnim.PlayTargetAnimation(CurrentAttack.ActionAnimation, true);
        // TODO: this is just to finish AI refactor
        // enemyManager.CurrentRecoveryTime = CurrentAttack.RecoveryTime;
        hasPerformedAttack = true;
    }

    private void RollForComboChance(EnemyManager enemyManager) {
        float comboChance = Random.Range(0, 100);

        if (!enemyManager.AllowAIToPerformCombos || comboChance > enemyManager.ComboChance) return;

        if (CurrentAttack.comboAction != null) {
            willComboNextAttack = true;
            CurrentAttack = CurrentAttack.comboAction;
        }
        else {
            willComboNextAttack = false;
            CurrentAttack = null;
        }
    }

    private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager, Vector3 direction) {
        if (enemyManager.CanRotate) {
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero) direction = enemyManager.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation,
                    enemyManager.RotationSpeed / Time.fixedDeltaTime);
        }
    }

    public void SetHasPerformedAttack(bool value) => hasPerformedAttack = value;
}
}