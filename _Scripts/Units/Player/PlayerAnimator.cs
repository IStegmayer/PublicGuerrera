using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator
{
    
}

public struct GoblinAnimHashes
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int IsMoving = Animator.StringToHash("IsMoving");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Damage = Animator.StringToHash("Damage");
    public static readonly int Death = Animator.StringToHash("Death");
}

public struct PlayerAnimHashes
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int IsIdle = Animator.StringToHash("IsIdle");
    public static readonly int IsWalking = Animator.StringToHash("IsWalking");
    public static readonly int IsRunning = Animator.StringToHash("IsRunning");
    public static readonly int IsJumping = Animator.StringToHash("IsJumping");
    public static readonly int IsBlocking = Animator.StringToHash("IsBlocking");
    public static readonly int JumpCount = Animator.StringToHash("JumpCount");
    public static readonly int Run = Animator.StringToHash("Run");
    public static readonly int Walk = Animator.StringToHash("Walk");
    public static readonly int Slash = Animator.StringToHash("Slash");
    public static readonly int Roll = Animator.StringToHash("Roll");
    public static readonly int Block = Animator.StringToHash("Block");
    public static readonly int Damage = Animator.StringToHash("Damage");
    public static readonly int Death = Animator.StringToHash("Death");
} 