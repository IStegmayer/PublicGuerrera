using UnityEngine;


namespace Game._Scripts.Player.StateMachine
{
    public class PlayerFallState : PlayerBaseState, IRootState
    {
        public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
            currentContext, playerStateFactory) {
            IsRootState = true;
        }

        public override void EnterState() {
            InitializeSubState();
        }

        public override void UpdateState() {
            HandleGravity();
            CheckSwitchStates();
        }

        public override void ExitState() {
            Debug.Log("ExitState Fall");
        }

        public override void CheckSwitchStates() {
            if (Ctx.CharacterController.isGrounded) {
                SwitchState(Factory.Grounded());
            }
        }

        public override void InitializeSubState() {
            if (!Ctx.IsMovementPressed && !Ctx.Input.Run) {
                SetSubState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && !Ctx.Input.Run) {
                SetSubState(Factory.Walk());
            }
            else {
                SetSubState((Factory.Run()));
            }
        }

        public void HandleGravity() {
            float previousYVelocity = Ctx.AppliedMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + Ctx.Gravity * Time.deltaTime;
            Ctx.AppliedMovementY =
                Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, Ctx.FallTerminalVelocity);
        }
    }
}