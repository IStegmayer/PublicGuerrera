using UnityEngine;

namespace Dzajna {
public class CombatStanceState : State {
    [SerializeField] private AttackState attackState;
    [SerializeField] private PursueTargetState pursueTargetState;
    public EnemyAttackAction[] EnemyAttacks;

    protected bool randomDestinationSet;
    protected float verticalMovementValue;
    protected float horizontalMovementValue;

    public override State Tick(EnemyManager enemy) {
        CharacterAnimatorManager enemyAnim = enemy.CharacterAnimatorManager;
        if (enemy.IsInteracting) {
            enemyAnim.Anim.SetFloat(enemyAnim.VerticalHash, 0);
            enemyAnim.Anim.SetFloat(enemyAnim.HorizontalHash, 0);
            return this;
        }

        attackState.SetHasPerformedAttack(false);

        enemyAnim.Anim.SetFloat(enemyAnim.VerticalHash, verticalMovementValue, 0.2f, Time.deltaTime);
        enemyAnim.Anim.SetFloat(enemyAnim.HorizontalHash, horizontalMovementValue, 0.2f, Time.deltaTime);

        if (enemy.DistanceFromTarget > enemy.MaxAttackRange)
            return pursueTargetState;

        if (!randomDestinationSet) {
            randomDestinationSet = true;
            DecideCirclingAction(enemyAnim);
        }

        HandleRotateTowardsTarget(enemy);

        if (enemy.CurrentRecoveryTime <= 0 && attackState.CurrentAttack != null) {
            randomDestinationSet = false;
            return attackState;
        }

        GetNewAttack(enemy);
        return this;
    }

    protected virtual void GetNewAttack(EnemyManager enemy) {
        int maxScore = 0;
        EnemyAttackAction[] attacks = GetEligibleAttacks();

        for (int i = 0; i < attacks.Length; i++) {
            EnemyAttackAction enemyAttackAction = attacks[i];

            // TODO: this is just to finish AI refactor
            // if (distanceFromTarget <= enemyAttackAction.MaxDistanceNeededToAttack &&
            //     distanceFromTarget >= enemyAttackAction.MinDistanceNeededToAttack) {
            //     if (viewableAngle <= enemyAttackAction.MaxAttackAngle &&
            //         viewableAngle >= enemyAttackAction.MinAttackAngle) {
            //         maxScore += enemyAttackAction.AttackScore;
            //     }
            // }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int j = 0; j < attacks.Length; j++) {
            EnemyAttackAction enemyAttackAction = attacks[j];

            // TODO: this is just to finish AI refactor
            // if (distanceFromTarget <= enemyAttackAction.MaxDistanceNeededToAttack &&
            //     distanceFromTarget >= enemyAttackAction.MinDistanceNeededToAttack) {
            //     if (viewableAngle <= enemyAttackAction.MaxAttackAngle &&
            //         viewableAngle >= enemyAttackAction.MinAttackAngle) {
            //         if (attackState.CurrentAttack != null) return;
            //
            //         temporaryScore += enemyAttackAction.AttackScore;
            //         if (temporaryScore > randomValue) attackState.CurrentAttack = enemyAttackAction;
            //     }
            // }
        }
    }

    protected virtual void HandleRotateTowardsTarget(EnemyManager enemy) {
        if (enemy.IsPerformingAction) {
            enemy.TargetsDirection.y = 0;
            enemy.TargetsDirection.Normalize();

            if (enemy.TargetsDirection == Vector3.zero) enemy.TargetsDirection = enemy.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(enemy.TargetsDirection);
            enemy.transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation,
                    enemy.RotationSpeed / Time.fixedDeltaTime);
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
                enemy.RotationSpeed / Time.fixedDeltaTime);
        }
    }

    protected virtual void DecideCirclingAction(CharacterAnimatorManager enemyAnim) {
        WalkAroundTarget(enemyAnim);
        // Circle with only forward vertical movement
        // Circle with running 
        // Circle with walking only
        // Don't circle
    }

    protected virtual void WalkAroundTarget(CharacterAnimatorManager enemyAnim) {
        float walkSpeed = 0.5f;
        float runSpeed = 1f;
        verticalMovementValue = walkSpeed; // To include backwards, values (-1,1)
        horizontalMovementValue = Random.Range(-1, 1);

        if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0) horizontalMovementValue = 0.5f;
        else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0) horizontalMovementValue = -0.5f;
    }

    protected virtual EnemyAttackAction[] GetEligibleAttacks() {
        return EnemyAttacks;
    }
}
}