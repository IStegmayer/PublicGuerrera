using UnityEngine;

namespace Dzajna {
public class PursueTargetStateHumanoid : State {
    [SerializeField] private CombatStanceStateHumanoid combatStanceState;

    public override State Tick(EnemyManager enemy) {
        if (enemy.IsInteracting) return this;

        CharacterAnimatorManager aiAnim = enemy.CharacterAnimatorManager;
        HandleRotateTowardsTarget(enemy);

        if (enemy.IsPerformingAction) {
            aiAnim.Anim.SetFloat(aiAnim.VerticalHash, 0, 0.1f, Time.deltaTime);
            return this;
        }

        if (enemy.DistanceFromTarget > enemy.MaxAttackRange)
            aiAnim.Anim.SetFloat(aiAnim.VerticalHash, 1, 0.1f, Time.deltaTime);


        // If within attack range, switch to combat stance state
        // If target is out of attack range, return this state and continue giving chase
        if (enemy.DistanceFromTarget <= enemy.MaxAttackRange) return combatStanceState;
        else return this;
    }

    private void HandleRotateTowardsTarget(EnemyManager enemyManager) {
        if (enemyManager.IsPerformingAction) {
            enemyManager.TargetsDirection.y = 0;
            enemyManager.TargetsDirection.Normalize();

            if (enemyManager.TargetsDirection == Vector3.zero)
                enemyManager.TargetsDirection = enemyManager.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(enemyManager.TargetsDirection);
            enemyManager.transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation,
                    enemyManager.RotationSpeed / Time.fixedDeltaTime);
        }
        else {
            Vector3 relativeDirection =
                transform.InverseTransformDirection(enemyManager.NavMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.Rigidbody.velocity;

            enemyManager.NavMeshAgent.enabled = true;
            enemyManager.NavMeshAgent.SetDestination(enemyManager.CurrentTarget.transform.position);
            enemyManager.Rigidbody.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation,
                enemyManager.NavMeshAgent.transform.rotation, enemyManager.RotationSpeed);
        }
    }
}
}