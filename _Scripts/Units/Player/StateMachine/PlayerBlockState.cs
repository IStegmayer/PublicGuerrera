using Game._Scripts;
using Game._Scripts.Player.StateMachine;

public class PlayerBlockState : PlayerBaseState, IRootState
{
    public PlayerBlockState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory) {
        IsRootState = true;
    }

    public override void EnterState() {
        Ctx.IsBlocking = true;
        Ctx.Animator.Play(PlayerAnimHashes.Block);
        Ctx.Animator.SetBool(PlayerAnimHashes.IsBlocking, true);
    }

    public override void UpdateState() {
        Ctx.AppliedMovementX = Ctx.Input.Movement.x * Ctx.WalkMultiplier;
        Ctx.AppliedMovementZ = Ctx.Input.Movement.y * Ctx.WalkMultiplier;
        CheckSwitchStates();
    }

    public override void ExitState() {
        //TODO: these fields could just be used as one: player animator as a mono and use 
        // the ctx fields to update the Anim props every frame?
        Ctx.IsBlocking = false;
        Ctx.Animator.SetBool(PlayerAnimHashes.IsBlocking, false);
    }

    public override void CheckSwitchStates() {
        if (!Ctx.Input.Block) SwitchState(Factory.Grounded());
    }

    public override void InitializeSubState() {
        // TODO: strafe
    }

    public void HandleGravity() {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }
}