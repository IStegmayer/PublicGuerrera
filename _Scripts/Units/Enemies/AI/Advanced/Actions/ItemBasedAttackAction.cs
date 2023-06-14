using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "A.I/Humanoid Actions/Item Based Attack Action")]
public class ItemBasedAttackAction : ScriptableObject {
    [Header("Attack Type")] 
    public AIAttackActionType AIAttackActionType = AIAttackActionType.MeleeAction;
    public AttackType AttackType = AttackType.Light;

    [Header("Action Combo Settings")] 
    public bool actionCanCombo;

    [Header("Action Hand")] 
    public bool isRightHandedAction;
    public bool isLeftHandedAction;
    
    [Header("Action Settings")]
    public int AttackScore = 3;
    public float RecoveryTime = 2;

    public float MaxAttackAngle = 35;
    public float MinAttackAngle = -35;

    public float MinDistanceNeededToAttack = 1;
    public float MaxDistanceNeededToAttack = 3;

    public void PerformAttackAction(EnemyManager enemyManager) {
        enemyManager.IsUsingRightHand = isRightHandedAction;
        enemyManager.IsUsingLeftHand = isLeftHandedAction;

        PerformItemActionBasedOnAttackType(enemyManager);
    }
    
    // Decide which hand performs action
    private void PerformItemActionBasedOnAttackType(EnemyManager enemy) {
        if (AIAttackActionType == AIAttackActionType.MeleeAction) {
            if (isRightHandedAction) PerformRightHandedMeleeAction(enemy);
            else PerformLeftHandedMeleeAction(enemy);
        }
        else if (AIAttackActionType == AIAttackActionType.MagicAction) {
            PerformMagicAction(enemy);
        }
    }
    private void PerformRightHandedMeleeAction(EnemyManager enemy) {
        if (enemy.IsTwoHandingWeapon) {
            if (AttackType == AttackType.Light) 
                enemy.CharacterInventoryManager.RightWeapon.TH_TapRBAction.PerformAction(enemy);
            else if (AttackType == AttackType.Heavy) 
                enemy.CharacterInventoryManager.RightWeapon.TH_TapRTAction.PerformAction(enemy);
        }
        else {
            if (AttackType == AttackType.Light) 
                enemy.CharacterInventoryManager.RightWeapon.TapRBAction.PerformAction(enemy);
            else if (AttackType == AttackType.Heavy) 
                enemy.CharacterInventoryManager.RightWeapon.TapRTAction.PerformAction(enemy);
        }
    }
    private void PerformLeftHandedMeleeAction(EnemyManager enemyManager) {
        throw new System.NotImplementedException();
    }
    private void PerformMagicAction(EnemyManager enemyManager) {
        throw new System.NotImplementedException();
    }
}
}