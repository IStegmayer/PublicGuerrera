using System.Collections;
using UnityEngine;

namespace Game._Scripts.Player.StateMachine
{
    public class PlayerAttackState : PlayerBaseState, IRootState
    {
        public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
            currentContext, playerStateFactory) {
            IsRootState = true;
        }
        
        private IEnumerator SlashResetRoutine() {
            yield return new WaitForSeconds(1f);
            // modify slash count
            Ctx.IsSlashing = false;
         }

        public override void EnterState() {
            Ctx.Weapon.ToggleEnableDamage(true);
            Ctx.IsSlashing = true;
            Ctx.Animator.Play(PlayerAnimHashes.Slash);
            Ctx.StartCoroutine(SlashResetRoutine());
        }

        public override void UpdateState() {
            Ctx.AppliedMovementX = 0.0f;
            Ctx.AppliedMovementZ = 0.0f;
            CheckSwitchStates();
        }

        public override void ExitState() {
            Ctx.Weapon.ToggleEnableDamage(false);
            //TODO: This is lazy
            Ctx.Animator.Play(PlayerAnimHashes.Idle);
        }

        public override void CheckSwitchStates() {
            if (!Ctx.IsSlashing) SwitchState(Factory.Grounded());
        }

        public override void InitializeSubState() {
        }
        public void HandleGravity() {
            Ctx.CurrentMovementY = Ctx.Gravity;
            Ctx.AppliedMovementY = Ctx.Gravity;
        }
    }
}