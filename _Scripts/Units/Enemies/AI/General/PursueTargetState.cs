using UnityEngine;

namespace Dzajna {
public class PursueTargetState : State {
    [SerializeField] private CombatStanceState combatStanceState;

    public override State Tick(EnemyManager enemy) {
        if (enemy.IsInteracting) return this;

        HandleRotateTowardsTarget(enemy);
        CharacterAnimatorManager enemyAnim = enemy.CharacterAnimatorManager;

        if (enemy.IsPerformingAction) {
            enemyAnim.Anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        // Chase the target
        float distanceFromTarget =
            Vector3.Distance(enemy.CurrentTarget.transform.position, enemy.transform.position);
        float viewableAngle = Vector3.SignedAngle(enemy.TargetsDirection, enemy.transform.forward, Vector3.up);
        if (distanceFromTarget > enemy.MaxAttackRange)
            enemyAnim.Anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);


        // If within attack range, switch to combat stance state
        // If target is out of attack range, return this state and continue giving chase
        if (distanceFromTarget <= enemy.MaxAttackRange) return combatStanceState;
        else return this;
    }

    private void HandleRotateTowardsTarget(EnemyManager enemy) {
        if (enemy.IsPerformingAction) {
            enemy.TargetsDirection.y = 0;
            enemy.TargetsDirection.Normalize();

            if (enemy.TargetsDirection == Vector3.zero) enemy.TargetsDirection = enemy.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(enemy.TargetsDirection);
            enemy.transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation,
                    enemy.RotationSpeed / Time.deltaTime);
        }
        else {
            Vector3 relativeDirection =
                transform.InverseTransformDirection(enemy.NavMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemy.Rigidbody.velocity;

            enemy.NavMeshAgent.enabled = true;
            enemy.NavMeshAgent.SetDestination(enemy.CurrentTarget.transform.position);
            enemy.Rigidbody.velocity = targetVelocity;
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation,
                enemy.NavMeshAgent.transform.rotation,
                enemy.RotationSpeed / Time.deltaTime);
        }
    }
}
}