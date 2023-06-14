using UnityEngine;

namespace Dzajna {
public class IdleState : State {
    [SerializeField] private PursueTargetState pursueTargetState;
    [SerializeField] private LayerMask layerMask;

    public override State Tick(EnemyManager enemy) {
        if (enemy.IsInteracting) return this;

        // Look for a potential target
        Collider[] colliders =
            Physics.OverlapSphere(enemy.transform.position, enemy.AggroRange, layerMask);

        for (int i = 0; i < colliders.Length; i++) {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if (targetCharacter != null) {
                if (enemy.ViewableAngle < enemy.MinDetectionAngle && enemy.ViewableAngle > enemy.MaxDetectionAngle) {
                    enemy.CurrentTarget = targetCharacter;
                }
            }
        }

        if (enemy.CurrentTarget != null) return pursueTargetState;
        else return this;
    }
}
}