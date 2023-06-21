using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Dzajna {
public class InputHandler : PersistentSingleton<InputHandler> {
    //TODO: This could be renamed and mapped to just the pressed buttons and a dictionary of 

    // MOVEMENT
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float MoveAmount { get; private set; }
    public float CameraX { get; private set; }

    public float CameraY { get; private set; }

    // BUTTONS
    public bool BInput { get; private set; }
    public bool AInput { get; private set; }
    public bool RBInput { get; private set; }
    public bool RBHoldInput { get; private set; }
    public bool LBInput { get; private set; }
    public bool LBHoldInput { get; private set; }
    public bool RTInput { get; private set; }
    public bool RTHoldInput { get; private set; }
    public bool LTInput { get; private set; }
    public bool CriticalInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool TwoHandInput { get; private set; }
    public bool InventoryInput { get; private set; }
    public bool LockOnInput { get; private set; }
    public bool LockOnLeftInput { get; private set; }

    public bool LockOnRightInput { get; private set; }

    // DPAD
    public bool DPadUpInput { get; private set; }
    public bool DPadDownInput { get; private set; }
    public bool DPadLeftInput { get; private set; }

    public bool DPadRightInput { get; private set; }

    // FLAGS AND TIMERS
    public bool RollFlag { get; private set; }
    public bool ComboFlag { get; private set; }
    public bool InventoryFlag { get; private set; }
    public bool LockOnFlag { get; private set; }
    public bool TwoHandFlag { get; private set; }
    private float rollInputTimer;

    [SerializeField] private PlayerManager player;
    private PlayerCharacterAnimatorManager playerCharacterAnimatorManager;
    private PlayerInputActions inputActions;
    private PlayerCombatManager playerCombatManager;
    private CharacterInventoryManager characterInventoryManager;
    private PlayerStatsManager playerStatsManager;
    private PlayerEquipmentManager playerEquipmentManager;
    private PlayerCamera playerCamera;
    private UIManager uiManager;
    private PlayerWeaponSlotManager playerWeaponSlotManager;

    private Vector2 movementInput;
    private Vector2 cameraInput;

    protected void Start() {
        playerCamera = PlayerCamera.Instance;
        playerCombatManager = player.GetComponent<PlayerCombatManager>();
        characterInventoryManager = player.GetComponent<CharacterInventoryManager>();
        playerCharacterAnimatorManager = player.GetComponent<PlayerCharacterAnimatorManager>();
        playerStatsManager = player.GetComponent<PlayerStatsManager>();
        playerWeaponSlotManager = player.GetComponentInChildren<PlayerWeaponSlotManager>();
        playerEquipmentManager = player.GetComponentInChildren<PlayerEquipmentManager>();
        // TODO: inefficient asf
        uiManager = FindObjectOfType<UIManager>();
    }

    public void OnEnable() {
        // read input and populate our fields
        if (inputActions == null) {
            inputActions = new PlayerInputActions();

            inputActions.PlayerMovement.Movement.performed += input => movementInput = input.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += input => cameraInput = input.ReadValue<Vector2>();

            inputActions.PlayerActions.HoldRB.performed += _ => RBHoldInput = true;
            inputActions.PlayerActions.HoldRB.canceled += _ => RBHoldInput = false;
            inputActions.PlayerActions.HoldLB.performed += _ => LBHoldInput = true;
            inputActions.PlayerActions.HoldLB.canceled += _ => LBHoldInput = false;
            inputActions.PlayerActions.HoldRT.performed += _ => RTHoldInput = true;
            inputActions.PlayerActions.HoldRT.canceled += _ => RTHoldInput = false;

            inputActions.PlayerActions.RB.performed += _ => RBInput = true;
            inputActions.PlayerActions.RT.performed += _ => RTInput = true;
            inputActions.PlayerActions.LT.performed += _ => LTInput = true;
            inputActions.PlayerActions.LB.performed += _ => LBInput = true;
            inputActions.PlayerActions.LB.canceled += _ => LBInput = false;
            inputActions.PlayerActions.Jump.performed += _ => JumpInput = true;
            inputActions.PlayerActions.A.performed += _ => AInput = true;
            inputActions.PlayerActions.B.performed += _ => BInput = true;
            inputActions.PlayerActions.B.canceled += _ => BInput = false;
            inputActions.PlayerActions.LockOn.performed += _ => LockOnInput = true;
            inputActions.PlayerActions.LockOnTargetLeft.performed += _ => LockOnLeftInput = true;
            inputActions.PlayerActions.LockOnTargetRight.performed += _ => LockOnRightInput = true;
            inputActions.PlayerActions.TwoHand.performed += _ => TwoHandInput = true;

            inputActions.PlayerControls.Inventory.performed += HandleInventoryInput;
            inputActions.PlayerControls.DPadRight.performed += _ => DPadRightInput = true;
            inputActions.PlayerControls.DPadLeft.performed += _ => DPadLeftInput = true;
        }

        // enable our created inputActions 
        inputActions.Enable();
    }

    public void TickInput(float delta) {
        if (player.IsDead) return;
        
        HandleMoveInput();
        HandleRollInput(delta);

        HandleHoldRBInput();
        HandleHoldLBInput();
        HandleHoldRTInput();

        HandleTapLBInput();
        HandleTapRBInput();
        HandleTapRTInput();
        HandleTapLTInput();

        HandleQuickSlotsInput();
        // handled differently
        // HandleInventoryInput();

        HandleLockOnInput();
        HandleLockOnSwitch();
        HandleTwoHandInput();
        // TODO: implement
        // HandleUseConsumableInput();
        // HandleQuedInput();
    }

    private void HandleTapLTInput() {
        if (!LTInput) return;

        LTInput = false;

        if (player.IsTwoHandingWeapon && characterInventoryManager.RightWeapon.TapLTAction != null) {
            //It will be the right handed weapon
            player.UpdateWhichHandCharacterIsUsing(true);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.RightWeapon;
            characterInventoryManager.RightWeapon.TapLTAction.PerformAction(player);
        }
        else if (characterInventoryManager.LeftWeapon.TapLTAction != null) {
            player.UpdateWhichHandCharacterIsUsing(false);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.LeftWeapon;
            characterInventoryManager.LeftWeapon.TapLTAction.PerformAction(player);
        }
    }


    private void HandleTapRTInput() {
        if (!RTInput) return;
        RTInput = false;

        if (characterInventoryManager.RightWeapon.TapRTAction == null) return;

        player.UpdateWhichHandCharacterIsUsing(true);
        characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.RightWeapon;
        characterInventoryManager.RightWeapon.TapRTAction.PerformAction(player);
    }

    private void HandleTapRBInput() {
        if (!RBInput) return;

        RBInput = false;
        if (player.IsTwoHandingWeapon && characterInventoryManager.RightWeapon.TH_TapRBAction != null) {
            player.UpdateWhichHandCharacterIsUsing(true);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.RightWeapon;
            characterInventoryManager.RightWeapon.TH_TapRBAction.PerformAction(player);
        }
        else if (characterInventoryManager.RightWeapon.TapRBAction != null) {
            player.UpdateWhichHandCharacterIsUsing(true);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.RightWeapon;
            characterInventoryManager.RightWeapon.TapRBAction.PerformAction(player);
        }
    }

    private void HandleTapLBInput() {
        if (!LBInput) return;
        LBInput = false;

        if (player.IsTwoHandingWeapon && characterInventoryManager.RightWeapon.TapLBAction != null) {
            player.UpdateWhichHandCharacterIsUsing(true);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.RightWeapon;
            characterInventoryManager.RightWeapon.TapLBAction.PerformAction(player);
        }
        else if (characterInventoryManager.LeftWeapon.TapLBAction != null) {
            player.UpdateWhichHandCharacterIsUsing(false);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.LeftWeapon;
            characterInventoryManager.LeftWeapon.TapLBAction.PerformAction(player);
        }
    }

    private void HandleHoldRTInput() {
        playerCharacterAnimatorManager.Anim.SetBool(playerCharacterAnimatorManager.IsChargingAttackHash, RTHoldInput);
        if (!RTHoldInput) return;

        player.UpdateWhichHandCharacterIsUsing(true);
        characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.RightWeapon;

        if (player.IsTwoHandingWeapon && characterInventoryManager.RightWeapon.TH_HoldRTAction != null)
            characterInventoryManager.RightWeapon.TH_HoldRTAction.PerformAction(player);
        else if (characterInventoryManager.RightWeapon.HoldRTAction != null)
            characterInventoryManager.RightWeapon.HoldRTAction.PerformAction(player);
    }

    private void HandleHoldLBInput() {
        if (!LBHoldInput) {
            if (player.IsBlocking) player.IsBlocking = false;
            return;
        }

        if (player.IsTwoHandingWeapon && characterInventoryManager.RightWeapon.TH_HoldRBAction != null) {
            player.UpdateWhichHandCharacterIsUsing(true);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.RightWeapon;
            characterInventoryManager.RightWeapon.TH_HoldRBAction.PerformAction(player);
        }
        else if (characterInventoryManager.RightWeapon.HoldLBAction != null) {
            player.UpdateWhichHandCharacterIsUsing(true);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.LeftWeapon;
            characterInventoryManager.LeftWeapon.HoldLBAction.PerformAction(player);
        }
        
    }

    private void HandleMoveInput() {
        Horizontal = movementInput.x;
        Vertical = movementInput.y;
        MoveAmount = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
        CameraX = cameraInput.x;
        CameraY = cameraInput.y;
    }

    private void HandleHoldRBInput() {
        if (!RBHoldInput) return;

        if (player.IsTwoHandingWeapon && characterInventoryManager.RightWeapon.TH_HoldRBAction != null) {
            player.UpdateWhichHandCharacterIsUsing(true);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.RightWeapon;
            characterInventoryManager.RightWeapon.TH_HoldRBAction.PerformAction(player);
        }
        else if (characterInventoryManager.RightWeapon.HoldRBAction != null) {
            player.UpdateWhichHandCharacterIsUsing(true);
            characterInventoryManager.CurrentItemBeingUsed = characterInventoryManager.RightWeapon;
            characterInventoryManager.RightWeapon.HoldRBAction.PerformAction(player);
        }
    }

    private void HandleRollInput(float delta) {
        if (BInput) {
            rollInputTimer += delta;
            bool hasStamina = playerStatsManager.CheckHasStamina();
            if (!hasStamina) {
                BInput = false;
                player.IsSprinting = false;
            }
            else if (MoveAmount > 0.5f) player.IsSprinting = true;
        }
        else {
            player.IsSprinting = false;
            if (rollInputTimer > 0 && rollInputTimer < 0.5f) RollFlag = true;

            rollInputTimer = 0;
        }
    }

    private void HandleQuickSlotsInput() {
        if (DPadRightInput) characterInventoryManager.ChangeRightWeapon();
        else if (DPadLeftInput) characterInventoryManager.ChangeLeftWeapon();
    }

    private void HandleInteractInput(CallbackContext callbackContext) {
        AInput = callbackContext.action.WasPerformedThisFrame();
    }

    private void HandleJumpInput(CallbackContext callbackContext) {
        JumpInput = callbackContext.action.WasPerformedThisFrame();
    }

    private void HandleInventoryInput(CallbackContext callbackContext) {
        InventoryInput = callbackContext.action.WasPerformedThisFrame();
        if (!InventoryInput) return;

        InventoryFlag = !InventoryFlag;
        uiManager.ToggleSelectWindow(InventoryFlag);
        if (InventoryFlag) uiManager.UpdateUI();
        else uiManager.CloseAllInventoryWindows();
        uiManager.ToggleHUDWindow(!InventoryFlag);
    }

    private void HandleLockOnInput() {
        if (!LockOnInput) return;

        if (!LockOnFlag) {
            LockOnInput = false;
            playerCamera.HandleLockOn();
            LockOnFlag = playerCamera.SetCurrentLockOnTarget();
        }
        else {
            LockOnInput = false;
            LockOnFlag = false;
            playerCamera.ClearLockOnTargets();
        }
    }

    private void HandleLockOnSwitch() {
        if (!LockOnFlag) return;
        if (!LockOnRightInput && !LockOnLeftInput) return;

        playerCamera.HandleLockOn();
        playerCamera.SwitchLockOnTarget(LockOnLeftInput, LockOnRightInput);
        LockOnLeftInput = false;
        LockOnRightInput = false;
    }

    private void HandleTwoHandInput() {
        if (!TwoHandInput) return;

        TwoHandInput = false;
        TwoHandFlag = !TwoHandFlag;
        if (TwoHandFlag) playerWeaponSlotManager.LoadWeaponOnSlot(characterInventoryManager.RightWeapon, false);
        else {
            playerWeaponSlotManager.LoadWeaponOnSlot(characterInventoryManager.RightWeapon, false);
            playerWeaponSlotManager.LoadWeaponOnSlot(characterInventoryManager.LeftWeapon, true);
        }
    }
    
    public void SetComboFlag(bool val) => ComboFlag = val;

    public void OnDisable() {
        // making sure to cleanup
        inputActions.Disable();
    }

    private void LateUpdate() {
        //reset flags
        RollFlag = false;
        RBInput = false;
        RTInput = false;
        LTInput = false;
        AInput = false;
        JumpInput = false;
        LockOnInput = false;
        DPadRightInput = false;
        DPadLeftInput = false;
        DPadUpInput = false;
        DPadDownInput = false;
    }
}
}
