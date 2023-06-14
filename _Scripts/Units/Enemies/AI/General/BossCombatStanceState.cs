using UnityEngine;

namespace Dzajna {
public class BossCombatStanceState : CombatStanceState {
    public bool HasPhaseShifted;
    public EnemyAttackAction[] SecondPhaseAttacks;

    protected override EnemyAttackAction[] GetEligibleAttacks() {
        if (HasPhaseShifted) return SecondPhaseAttacks;
        else return EnemyAttacks;
    }
}
}