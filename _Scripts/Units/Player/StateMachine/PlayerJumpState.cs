using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;


namespace Game._Scripts.Player.StateMachine
{
    public class PlayerJumpState : PlayerBaseState, IRootState
    {
        public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) {
            IsRootState = true;
        }

        private IEnumerator JumpResetRoutine() {
            yield return new WaitForSeconds(.5f);
            Ctx.JumpCount = 0;
        }
        
        public override void EnterState() {
            InitializeSubState();
            if (Ctx.Input.Jump) HandleJump();
        }

        public override void UpdateState() {
            CheckSwitchStates();
            HandleGravity();
            
            Ctx.Animator.SetInteger(PlayerAnimHashes.JumpCount, Ctx.JumpCount);
        }

        public override void ExitState() {
            Ctx.CurrentJumpResetCoroutine = Ctx.StartCoroutine(JumpResetRoutine());
            if (Ctx.JumpCount == 3) Ctx.JumpCount = 0;
            Ctx.Animator.SetBool(PlayerAnimHashes.IsJumping, false);
        }

        public override void CheckSwitchStates() {
            if (Ctx.CharacterController.isGrounded) {
                SwitchState(Factory.Grounded());
            }
        }

        public override void InitializeSubState() {
            
        }

        private void HandleJump() {
            Ctx.Animator.SetBool(PlayerAnimHashes.IsJumping, true);
            if (Ctx.JumpCount < 3 && Ctx.CurrentJumpResetCoroutine != null) {
                Ctx.StopCoroutine(Ctx.CurrentJumpResetCoroutine);
                Ctx.CurrentJumpResetCoroutine = null;
            }
            
            Ctx.JumpCount++;
            Ctx.IsJumping = true;
            Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
            Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
        }

        public void HandleGravity() {
            bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.Input.Jump;
            float fallMultiplier = 2.5f;
            if (isFalling) {
                float previousYVelocity = Ctx.CurrentMovementY;
                Ctx.CurrentMovementY += (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
                Ctx.AppliedMovementY = MathF.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, Ctx.FallTerminalVelocity);
            }
            else {
                float previousYVelocity = Ctx.CurrentMovementY;
                Ctx.CurrentMovementY += (Ctx.JumpGravities[Ctx.JumpCount] * Time.deltaTime);
                Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * .5f;
            }
        }
    }
}
