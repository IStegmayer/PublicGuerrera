using UnityEngine;

namespace Dzajna {
public class CombatStanceStateHumanoid : State {
    [SerializeField] private AttackStateHumanoid attackState;
    [SerializeField] private PursueTargetStateHumanoid pursueTargetState;
    public ItemBasedAttackAction[] EnemyAttacks;

    [Header("State Flags")] protected bool willBlock;
    protected bool willDodge;
    protected bool willParry;

    protected bool randomDestinationSet;
    protected float verticalMovementValue;
    protected float horizontalMovementValue;

    public override State Tick(EnemyManager enemy) {
        if (enemy.combatStyle == HumanAICombatStyle.Melee)
            return ProcessMeleeCombatStyle(enemy);
        else if (enemy.combatStyle == HumanAICombatStyle.Shaman)
            return ProcessShamanCombatStyle(enemy);

        return this;
    }

    private State ProcessMeleeCombatStyle(EnemyManager enemy) {
        CharacterAnimatorManager aiAnim = enemy.CharacterAnimatorManager;

        // If AI is not falling or actioning stop all movement
        if (enemy.IsInteracting || !enemy.IsGrounded) {
            aiAnim.Anim.SetFloat(aiAnim.VerticalHash, 0);
            aiAnim.Anim.SetFloat(aiAnim.HorizontalHash, 0);
            return this;
        }

        // If AI too far from the target, return it to it's pursue target state
        if (enemy.DistanceFromTarget > enemy.MaxAggroRadius) return pursueTargetState;

        // Randomizes walking/strafe pattern of the AI
        if (!randomDestinationSet) {
            randomDestinationSet = true;
            DecideCirclingAction(aiAnim);
        }

        // TODO: Use a curve?

        // roll for a block chance
        if (enemy.allowAIToBlock) {
            willBlock = RollForDefensiveActionChance(enemy.blockLikelihood);
            if (willBlock) { } // block using off hand 
        }

        // roll for a dodge chance
        if (enemy.allowAIToDodge) {
            willDodge = RollForDefensiveActionChance(enemy.dodgeLikelihood);
            if (willDodge) { } // if enemy is attacking the AI, dodge
        }

        // roll for a parry chance
        if (enemy.allowAIToParry) {
            willParry = RollForDefensiveActionChance(enemy.parryLikelihood);
            if (willParry) { } // if enemy is attacking and is parriable, parry
        }

        if (enemy.CanRotate) HandleRotateTowardsTarget(enemy);

        if (enemy.CurrentRecoveryTime <= 0 && attackState.CurrentAttack != null) {
            ResetStateFlags();
            return attackState;
        }

        GetNewAttack(enemy);
        HandleMovement(enemy, aiAnim);

        return this;
    }

    protected virtual void GetNewAttack(EnemyManager enemy) {
        int maxScore = 0;
        ItemBasedAttackAction[] attacks = GetEligibleAttacks();

        for (int i = 0; i < attacks.Length; i++) {
            ItemBasedAttackAction enemyAttackAction = attacks[i];

            if (enemy.DistanceFromTarget <= enemyAttackAction.MaxDistanceNeededToAttack &&
                enemy.DistanceFromTarget >= enemyAttackAction.MinDistanceNeededToAttack) {
                if (enemy.ViewableAngle <= enemyAttackAction.MaxAttackAngle &&
                    enemy.ViewableAngle >= enemyAttackAction.MinAttackAngle) {
                    maxScore += enemyAttackAction.AttackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int j = 0; j < attacks.Length; j++) {
            ItemBasedAttackAction enemyAttackAction = attacks[j];

            if (enemy.DistanceFromTarget <= enemyAttackAction.MaxDistanceNeededToAttack &&
                enemy.DistanceFromTarget >= enemyAttackAction.MinDistanceNeededToAttack) {
                if (enemy.ViewableAngle <= enemyAttackAction.MaxAttackAngle &&
                    enemy.ViewableAngle >= enemyAttackAction.MinAttackAngle) {
                    if (attackState.CurrentAttack != null) return;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         

                    temporaryScore += enemyAttackAction.AttackScore;
                    if (temporaryScore > randomValue) attackState.CurrentAttack = enemyAttackAction;
                }
            }
        }
    }

    private State ProcessShamanCombatStyle(EnemyManager enemy) {
        //If the A.I is falling, or is performing some sort of action STOP all movement
        if (!enemy.IsGrounded || enemy.IsInteracting) {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);
            return this;
        }

        //If the A.I has gotten too far from it's target, return the A.I to it's pursue target state
        if (enemy.DistanceFromTarget > enemy.MaxAggroRadius) {
            ResetStateFlags();
            return pursueTargetState;
        }

        //Randomizes the walking pattern of our A.I so they circle the player
        if (!randomDestinationSet) {
            randomDestinationSet = true;
            DecideCirclingAction(enemy.CharacterAnimatorManager);
        }

        HandleRotateTowardsTarget(enemy);

        if (enemy.CurrentRecoveryTime <= 0) {
            ResetStateFlags();
            return attackState;
        }

        enemy.animator.SetFloat(enemy.CharacterAnimatorManager.VerticalHash, 0, 0.2f, Time.deltaTime);
        enemy.animator.SetFloat(enemy.CharacterAnimatorManager.HorizontalHash, 0, 0.2f, Time.deltaTime);
        
        return this;
    }

    protected void HandleRotateTowardsTarget(EnemyManager enemy) {
        var direction = enemy.TargetsDirection;
        direction.y = 0;
        direction.Normalize();
        
        if (direction == Vector3.zero) direction = transform.forward;
        
        var targetRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.RotationSpeed);
    }

    protected virtual void DecideCirclingAction(CharacterAnimatorManager aiAnim) {
        // Circle with walking only
        WalkAroundTarget(aiAnim);
        // Circle with only forward vertical movement
        // Circle with running 
        // Don't circle
    }

    protected virtual void WalkAroundTarget(CharacterAnimatorManager aiAnim) {
        float walkSpeed = 0.5f;
        float runSpeed = 1f;
        verticalMovementValue = walkSpeed; // To include backwards, values (-0.5,0.5)
        horizontalMovementValue = Random.Range(-runSpeed, runSpeed);

        if (horizontalMovementValue <= runSpeed && horizontalMovementValue >= 0)
            horizontalMovementValue = walkSpeed;
        else if (horizontalMovementValue >= -runSpeed && horizontalMovementValue < 0)
            horizontalMovementValue = -walkSpeed;
    }

    private bool RollForDefensiveActionChance(float likelihood) {
        if (likelihood <= Random.Range(0, 100)) return true;
        return false;
    }

    private void HandleMovement(EnemyManager enemy, CharacterAnimatorManager aiAnim) {
        if (enemy.DistanceFromTarget <= enemy.StoppingDistance) {
            aiAnim.Anim.SetFloat(aiAnim.VerticalHash, 0, 0.2f, Time.deltaTime);
            aiAnim.Anim.SetFloat(aiAnim.HorizontalHash, horizontalMovementValue, 0.2f, Time.deltaTime);
        }
        else {
            aiAnim.Anim.SetFloat(aiAnim.VerticalHash, verticalMovementValue, 0.2f, Time.deltaTime);
            aiAnim.Anim.SetFloat(aiAnim.HorizontalHash, horizontalMovementValue, 0.2f, Time.deltaTime);
        }
    }

    private void ResetStateFlags() {
        randomDestinationSet = false;
        willBlock = false;
        willDodge = false;
        willParry = false;
    }

    protected virtual ItemBasedAttackAction[] GetEligibleAttacks() {
        return EnemyAttacks;
    }
}
}