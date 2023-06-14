using UnityEngine;


namespace Game._Scripts.Player.StateMachine
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { }

        public override void EnterState() {
            Ctx.Animator.SetBool(PlayerAnimHashes.IsWalking, false);
            Ctx.Animator.SetBool(PlayerAnimHashes.IsRunning, false);
        }

        public override void UpdateState() {
            Ctx.AppliedMovementX = 0.0f;
            Ctx.AppliedMovementZ = 0.0f;
            CheckSwitchStates();
        }

        public override void ExitState() { }

        public override void CheckSwitchStates() {
            if (Ctx.IsMovementPressed && Ctx.Input.Run) {
                SwitchState(Factory.Run());
            }
            else if (Ctx.IsMovementPressed) {
                SwitchState(Factory.Walk());
            }
        }

        public override void InitializeSubState() {
            throw new System.NotImplementedException();
        }
    }
}