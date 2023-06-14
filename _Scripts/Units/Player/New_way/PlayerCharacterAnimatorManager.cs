using System;
using UnityEngine;

namespace Dzajna {
public class PlayerCharacterAnimatorManager : CharacterAnimatorManager {
    // TODO: public?
    private PlayerManager playerManager;
    private PlayerStatsManager playerStatsManager;


    protected override void Awake() {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        Anim = GetComponent<Animator>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        CanRotate = true;
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting) {
        //clamp values
        // #region Vertical
        //
        // float vertical = 0;
        //
        // if (verticalMovement > 0 && verticalMovement < 0.55f) vertical = 0.5f;
        // else if (verticalMovement > 0.55f) vertical = 1f;
        // else if (verticalMovement < 0 && verticalMovement > -0.55f) vertical = -0.5f;
        // else if (verticalMovement < -0.55f) vertical = -1f;
        // else vertical = 0f;
        //
        // #endregion
        // #region Horizontal
        //
        // float horizontal = 0;
        //
        // if (horizontalMovement > 0 && horizontalMovement < 0.55f) horizontal = 0.5f;
        // else if (horizontalMovement > 0.55f) horizontal = 1f;
        // else if (horizontalMovement < 0 && horizontalMovement > -0.55f) horizontal = -0.5f;
        // else if (horizontalMovement < -0.55f) horizontal = -1f;
        // else horizontal = 0f;
        // #endregion

        float horizontal = horizontalMovement;
        float vertical = verticalMovement;

        if (isSprinting) {
            vertical = 2;
            horizontal = horizontalMovement;
        }

        Anim.SetFloat(VerticalHash, vertical, 0.1f, Time.deltaTime);
        Anim.SetFloat(HorizontalHash, horizontal, 0.1f, Time.deltaTime);
        Anim.applyRootMotion = playerManager.IsInteracting;
    }

    public void AwardSoulsOnDeath() {
        //TODO:  Placeholder for enemynimator event.
    }

    public override void TakeCriticalDamageAnimEvent() {
        playerStatsManager.TakeDamageWithoutAnimation(playerManager.PendingCriticalDamage);
        playerManager.PendingCriticalDamage = 0;
    }
}
}