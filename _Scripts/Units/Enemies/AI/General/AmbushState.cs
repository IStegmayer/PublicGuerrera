using UnityEngine;

namespace Dzajna {
public class AmbushState : State {
    public bool IsSleeping;
    [SerializeField] private float detectionRadius = 20;
    [SerializeField] private string sleepAnimation;
    [SerializeField] private string wakeAnimation;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemy) {
        if (enemy.IsInteracting) return this;
        CharacterAnimatorManager enemyAnim = enemy.CharacterAnimatorManager;
        if (IsSleeping) enemyAnim .PlayTargetAnimation(sleepAnimation, true);

        HandleTargetDetection(enemy, enemyAnim);
        if (enemy.CurrentTarget != null) return pursueTargetState;
        else return this;
    }

    private void HandleTargetDetection(EnemyManager enemy, CharacterAnimatorManager enemyAnim) {
        Collider[] colliders =
            Physics.OverlapSphere(enemy.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++) {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
            if (targetCharacter == null) continue;

            if (enemy.ViewableAngle < enemy.MinDetectionAngle && enemy.ViewableAngle > enemy.MaxDetectionAngle) {
                enemy.CurrentTarget = targetCharacter;
                IsSleeping = false;
                enemyAnim.PlayTargetAnimation(wakeAnimation, true);
            }
        }
    }
}
}