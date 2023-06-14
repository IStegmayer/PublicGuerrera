using UnityEngine;

namespace Dzajna {
public class IdleStateHumanoid : State {
    [SerializeField] private PursueTargetStateHumanoid pursueTargetState;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask lineOfSightBlockingLayers;

    public override State Tick(EnemyManager enemy) {
        if (enemy.IsInteracting) return this;

        // Look for a potential target within detection radius
        Collider[] colliders =
            Physics.OverlapSphere(enemy.transform.position, enemy.AggroRange, layerMask);

        for (int i = 0; i < colliders.Length; i++) {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
            if (targetCharacter == null) continue;

            var targetDirection = targetCharacter.transform.position - transform.position;
            var viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            // if target is found, check that it's in front of the AI
            if (viewableAngle < enemy.MinDetectionAngle && viewableAngle > enemy.MaxDetectionAngle) {
                // if the AI's potential target has an obstruction in between itself and the AI, we do not add it as our current target
                Debug.DrawLine(enemy.LockOnTransform.position, targetCharacter.LockOnTransform.position,
                    Color.blue, 2f);
                if (Physics.Linecast(enemy.LockOnTransform.position,
                        targetCharacter.LockOnTransform.position, lineOfSightBlockingLayers))
                    return this;
                enemy.CurrentTarget = targetCharacter;
            }
        }

        if (enemy.CurrentTarget != null) return pursueTargetState;
        else return this;
    }
}
}