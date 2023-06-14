using System;
using System.Collections;
using UnityEngine;

namespace Dzajna {
public class PlayerManager : CharacterManager {
    private Animator anim;
    private PlayerCharacterAnimatorManager playerCharacterAnimatorManager;
    private InputHandler inputHandler;
    private PlayerLocomotionManager playerLocomotionManager;
    private PlayerStatsManager playerStatsManager;

    private InteractableUI interactableUI;

    [Header("Interact settings")] 
    [SerializeField] private float interactRange;
    [SerializeField] private LayerMask interactableLayer;

    protected override void Awake() {
        base.Awake();
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    protected override void Start() {
        base.Start();
        inputHandler = InputHandler.Instance;
        playerCharacterAnimatorManager = GetComponent<PlayerCharacterAnimatorManager>();
        anim = GetComponent<Animator>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        BackStabCollider = GetComponentInChildren<CriticalDamageCollider>();
    }

    private void Update() {
        float delta = Time.deltaTime;
        IsInteracting = anim.GetBool(playerCharacterAnimatorManager.IsInteractingHash);
        CanDoCombo = anim.GetBool(playerCharacterAnimatorManager.CanDoComboHash);
        IsInvulnerable = anim.GetBool(playerCharacterAnimatorManager.IsInvulnerableHash);
        IsFiringSpell = anim.GetBool(playerCharacterAnimatorManager.IsFiringSpellHash);
        playerCharacterAnimatorManager.CanRotate = anim.GetBool(playerCharacterAnimatorManager.CanRotateHash);
        playerCharacterAnimatorManager.Anim.SetBool(playerCharacterAnimatorManager.IsDeadHash, IsDead);
        playerCharacterAnimatorManager.Anim.SetBool(playerCharacterAnimatorManager.IsBlockingHash, IsBlocking);
        playerCharacterAnimatorManager.Anim.SetBool(playerCharacterAnimatorManager.IsTwoHandingWeaponHash, IsTwoHandingWeapon);

        inputHandler.TickInput(delta);

        playerLocomotionManager.HandleGroundMovement(delta);
        playerLocomotionManager.HandleRotation(delta);
        playerLocomotionManager.HandleRolling(delta);
        playerLocomotionManager.HandleJumping();
        
        CheckForInteractableObject();

        playerStatsManager.RegenerateStamina();
    }

    private void FixedUpdate() {
    }

    //TODO: This needs to change anyway 
    private void CheckForInteractableObject() {
        Interactable interactable;
        RaycastHit hit;

        Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, interactRange, interactableLayer);
        // TODO: inefficient
        if (!hit.transform || !hit.transform.TryGetComponent(out interactable)) {
            if (interactableUI.InteractableUIGameObject != null)
                interactableUI.InteractableUIGameObject.SetActive(false);
            return;
        }

        string interactableText = interactable.InteractableText;
        interactableUI.InteractableText.text = interactableText;
        interactableUI.InteractableUIGameObject.SetActive(true);

        if (inputHandler.AInput) {
            interactable.Interact(this);
        }
    }

    public void OpenChestInteraction(Transform playerStandingPoint) {
        transform.position = playerStandingPoint.transform.position;
        playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.OpenChestHash, true);
    }

    public void PassThroughFogWallInteraction(Transform fogWallEntrance) {
        Vector3 rotationDirection = fogWallEntrance.transform.forward;
        Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = turnRotation;

        playerCharacterAnimatorManager.PlayTargetAnimation(playerCharacterAnimatorManager.PassThroughFogWallHash, true);
    }
}
}