using System;
using UnityEngine;


namespace Game._Scripts.Player.StateMachine
{
    public class PlayerGroundedState : PlayerBaseState, IRootState
    {
        public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) {
            IsRootState = true;
        }

        public override void EnterState() {
            InitializeSubState();
            HandleGravity();
        }

        public override void UpdateState() {
            CheckSwitchStates();
        }

        public override void ExitState() {
            Debug.Log("Exiting CurrentGameState grounded");
        }

        public override void CheckSwitchStates() {
            if (Ctx.Input.Attack) {
                SwitchState(Factory.Attack());
            }
            else if (Ctx.Input.Block) {
                SwitchState(Factory.Block());
            }
            else if (Ctx.IsMovementPressed && Ctx.Input.Roll) {
                SwitchState(Factory.Roll());
            }
            else if (Ctx.Input.Jump) {
                SwitchState(Factory.Jump());
            }
            else if (!Ctx.CharacterController.isGrounded) {
                SwitchState(Factory.Fall());
            }
        }

        public override void InitializeSubState() {
            if (!Ctx.IsMovementPressed && !Ctx.Input.Run) {
                Ctx.Animator.SetBool(PlayerAnimHashes.IsRunning, false);
                Ctx.Animator.SetBool(PlayerAnimHashes.IsWalking, false);
                SetSubState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && !Ctx.Input.Run) {
                Ctx.Animator.SetBool(PlayerAnimHashes.IsWalking, true);
                Ctx.Animator.SetBool(PlayerAnimHashes.IsRunning, false);
                SetSubState(Factory.Walk());
            }
            else {
                Ctx.Animator.SetBool(PlayerAnimHashes.IsRunning, true);
                Ctx.Animator.SetBool(PlayerAnimHashes.IsWalking, false);
                SetSubState((Factory.Run()));
            }
        }

        public void HandleGravity() {
            Ctx.CurrentMovementY = Ctx.Gravity;
            Ctx.AppliedMovementY = Ctx.Gravity;
        }
    }
}