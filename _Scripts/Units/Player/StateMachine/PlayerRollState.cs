using System.Collections;
using Game._Scripts;
using Game._Scripts.Player.StateMachine;
using UnityEngine;

public class PlayerRollState : PlayerBaseState, IRootState
{
    public PlayerRollState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory) {
        IsRootState = true;
    }

    private float _rollDirectionX;
    private float _rollDirectionZ;

    private IEnumerator RollResetRoutine() {
        yield return new WaitForSeconds(0.5f);
        // modify slash count
        Ctx.IsRolling = false;
    }

    public override void EnterState() {
        Ctx.IsRolling = true;
        Ctx.Animator.Play(PlayerAnimHashes.Roll);
        _rollDirectionX = Ctx.Input.Movement.x;
        _rollDirectionZ = Ctx.Input.Movement.y;
        Ctx.StartCoroutine(RollResetRoutine());
    }

    public override void UpdateState() {
        //TODO: make this configurable
        float rollSpeed = 10f;
        Ctx.AppliedMovementX = _rollDirectionX * rollSpeed;
        Ctx.AppliedMovementZ = _rollDirectionZ * rollSpeed;
        CheckSwitchStates();
    }

    public override void ExitState() {
        //TODO: This is lazy
        Ctx.Animator.Play(PlayerAnimHashes.Idle);
    }

    public override void CheckSwitchStates() {
        if (!Ctx.IsRolling) SwitchState(Factory.Grounded());
    }

    public override void InitializeSubState() { }

    public void HandleGravity() {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }
}