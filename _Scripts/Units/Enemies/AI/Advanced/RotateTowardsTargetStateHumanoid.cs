using UnityEngine;

namespace Dzajna {
public class RotateTowardsTargetStateHumanoid : State {
    [SerializeField] private CombatStanceStateHumanoid combatStanceState;

    public override State Tick(EnemyManager enemy) {
        enemy.animator.SetFloat("Vertical", 0);
        enemy.animator.SetFloat("Horizontal", 0);
        
        if (enemy.IsInteracting) return this; //When we enter the state we will still be interacting from the attack animation so we pause here until it has finished
        
        //TODO: refactor this
        CharacterAnimatorManager enemyAnim = enemy.CharacterAnimatorManager;
        if (enemy.ViewableAngle >= 100 && enemy.ViewableAngle <= 180) {
            enemyAnim.PlayTargetAnimationWithRootRotation(enemyAnim.TurnAroundHash, true);
            return combatStanceState;
        }
        
        if (enemy.ViewableAngle <= -101 && enemy.ViewableAngle >= -180) {
            enemyAnim.PlayTargetAnimationWithRootRotation(enemyAnim.TurnAroundHash, true);
            return combatStanceState;
        }
        
        if (enemy.ViewableAngle <= -45 && enemy.ViewableAngle >= -100) {
            enemyAnim.PlayTargetAnimationWithRootRotation(enemyAnim.TurnRightHash, true);
            return combatStanceState;
        }
        
        if (enemy.ViewableAngle >= 45 && enemy.ViewableAngle <= 100) {
            enemyAnim.PlayTargetAnimationWithRootRotation(enemyAnim.TurnLeftHash, true);
            return combatStanceState;
        }

        return combatStanceState;
    }
}
}