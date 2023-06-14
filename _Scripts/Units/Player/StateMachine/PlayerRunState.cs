using UnityEngine;


namespace Game._Scripts.Player.StateMachine
{
    public class PlayerRunState : PlayerBaseState
    {
        public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) {
        }

        public override void EnterState() {
            Ctx.Animator.SetBool(PlayerAnimHashes.IsRunning, true);
        }

        public override void UpdateState() {
            Ctx.AppliedMovementX = Ctx.Input.Movement.x * Ctx.RunMultiplier;
            Ctx.AppliedMovementZ = Ctx.Input.Movement.y * Ctx.RunMultiplier;
            CheckSwitchStates();
        }

        public override void ExitState() {
            Ctx.Animator.SetBool(PlayerAnimHashes.IsRunning, false);
        }

        public override void CheckSwitchStates() {
            if (!Ctx.IsMovementPressed) {
                SwitchState(Factory.Idle());
            }
            else if (!Ctx.Input.Run) {
                SwitchState(Factory.Walk());
            }
        }

        public override void InitializeSubState() {
            throw new System.NotImplementedException();
        }
    }
}