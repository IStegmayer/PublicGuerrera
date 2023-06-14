using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction {
    public bool canCombo;
    public EnemyAttackAction comboAction;
}
}