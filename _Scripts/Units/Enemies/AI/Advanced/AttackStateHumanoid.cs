using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dzajna {
public class AttackStateHumanoid : State {
    [SerializeField] private CombatStanceStateHumanoid combatStanceState;
    [SerializeField] private PursueTargetStateHumanoid pursueTargetState;
    [SerializeField] private RotateTowardsTargetStateHumanoid rotateTowardsTargetState;
    public ItemBasedAttackAction CurrentAttack;
    
    private bool willComboNextAttack;
    private bool hasPerformedAttack;

    private void OnEnable() => CharacterAnimatorManager.ComboDisabled += ResetStateFlags;
    private void OnDisable() => CharacterAnimatorManager.ComboDisabled -= ResetStateFlags;

    public override State Tick(EnemyManager enemy) {
        if (enemy.combatStyle == HumanAICombatStyle.Melee)
            return ProcessSwordAndShieldCombatStyle(enemy);
        if (enemy.combatStyle == HumanAICombatStyle.Shaman)
            return ProcessShamanCombatStyle(enemy);
        return this;
    }

    private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy) {
        RotateTowardsTargetWhilstAttacking(enemy);

        if (enemy.DistanceFromTarget > enemy.MaxAggroRadius) return pursueTargetState;

        if (willComboNextAttack && enemy.CanDoCombo) AttackTargetWithCombo(enemy);

        if (!hasPerformedAttack) {
            AttackTarget(enemy);
            RollForComboChance(enemy);
        }

        if (willComboNextAttack && hasPerformedAttack) return this; //GOES BACK UP TO PREFORM THE COMBO
        
        ResetStateFlags();
        
        return rotateTowardsTargetState;
    }

    private State ProcessShamanCombatStyle(EnemyManager enemy) {
        RotateTowardsTargetWhilstAttacking(enemy);

        if (enemy.IsInteracting) return this;

        if (enemy.IsFiringSpell) {
            ResetStateFlags();
            return combatStanceState;
        }

        if (enemy.CurrentTarget.IsDead) {
            ResetStateFlags();
            enemy.CurrentTarget = null;
            return this;
        }

        if (enemy.DistanceFromTarget > enemy.MaxAggroRadius) {
            ResetStateFlags();
            return pursueTargetState;
        }

        if (!hasPerformedAttack) {
            AttackTarget(enemy);
            return this;
        }

        ResetStateFlags();

        return rotateTowardsTargetState;
    }

    private void RollForComboChance(EnemyManager enemy) {
        float comboChance = Random.Range(0, 100);

        if (!enemy.AllowAIToPerformCombos || comboChance > enemy.ComboChance) return;

        if (CurrentAttack.actionCanCombo) willComboNextAttack = true;
        else {
            willComboNextAttack = false;
            CurrentAttack = null;
        }
    }

    private void AttackTarget(EnemyManager enemy) {
        CurrentAttack.PerformAttackAction(enemy);
        enemy.CurrentRecoveryTime = CurrentAttack.RecoveryTime;
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(EnemyManager enemy) {
        CurrentAttack.PerformAttackAction(enemy);
        willComboNextAttack = false;
        enemy.CurrentRecoveryTime = CurrentAttack.RecoveryTime;
        CurrentAttack = null;
    }

    private void RotateTowardsTargetWhilstAttacking(EnemyManager enemy) {
        //Rotate manually
        if (enemy.CanRotate && enemy.IsInteracting) {
            var direction = enemy.TargetsDirection;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero) direction = transform.forward;

            var targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                enemy.RotationSpeed / Time.deltaTime);
        }
    }

    private void ResetStateFlags() {
        willComboNextAttack = false;
        hasPerformedAttack = false;
    }
}
}