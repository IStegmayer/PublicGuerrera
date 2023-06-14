using System;
using UnityEngine;

namespace Dzajna {
public class CharacterLocomotionManager : MonoBehaviour {
    public Vector3 moveDirection;
    protected CharacterController characterController;
    protected CharacterManager character;
    protected CharacterAnimatorManager CharacterAnimatorManager;
    
    [Header("Gravity Settings")] 
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] protected Vector3 fallVelocity;
    [SerializeField] protected float fallStartYVelocity = -7f;
    [SerializeField] protected float groundedYVelocity = -20f;
    [SerializeField] protected float gravityForce = -9.8f;
    [SerializeField] protected float groundCheckSphereRadius = 0.2f;
    protected bool fallingVelocitySet = false;
    
    public float InAirTimer;

    protected virtual void Awake() {
    }

    protected virtual void Start() {
        characterController = GetComponent<CharacterController>();
        character = GetComponent<CharacterManager>();
        CharacterAnimatorManager = character.GetComponent<CharacterAnimatorManager>();
    }
    
    protected virtual void Update() {
        character.IsGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        CharacterAnimatorManager.Anim.SetBool(CharacterAnimatorManager.IsGroundedHash, character.IsGrounded);
        HandleGroundCheck();
    }

    public virtual void HandleGroundCheck() {
        if (character.IsGrounded && fallVelocity.y < 0) {
            InAirTimer = 0;
            fallingVelocitySet = false;
            fallVelocity.y = groundedYVelocity;
        }
        else {
            if (!fallingVelocitySet) {
                fallingVelocitySet = true;
                fallVelocity.y = fallStartYVelocity;
            }

            InAirTimer = InAirTimer + Time.deltaTime;
            fallVelocity.y += gravityForce * Time.deltaTime;
        }

        CharacterAnimatorManager.Anim.SetFloat(CharacterAnimatorManager.InAirTimer, InAirTimer);
        character.CharacterController.Move(fallVelocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawSphere(transform.position, groundCheckSphereRadius);
    }
}
}