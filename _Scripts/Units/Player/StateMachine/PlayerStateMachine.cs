using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dzajna;
using Game._Scripts.Player;
using Game._Scripts.Player.StateMachine;
using UnityEngine.InputSystem;


namespace Game._Scripts
{
    // CONTEXT CLASS
    public class PlayerStateMachine : BaseUnit
    {
        #region Public Fields

        public PlayerBaseState CurrentState {
            get => _currentState;
            set => _currentState = value;
        }

        public FrameInput Input {
            get => _input;
        }

        public Animator Animator {
            get => _animator;
        }

        public Coroutine CurrentJumpResetCoroutine {
            get => _currentJumpResetCoroutine;
            set => _currentJumpResetCoroutine = value;
        }

        public Dictionary<int, float> InitialJumpVelocities {
            get => _initialJumpVelocities;
        }

        public int JumpCount {
            get => _jumpCount;
            set => _jumpCount = value;
        }

        public bool IsJumping {
            get => _isJumping;
            set => _isJumping = value;
        }

        public float CurrentMovementY {
            get => _currentMovement.y;
            set => _currentMovement.y = value;
        }

        public float AppliedMovementY {
            get => _appliedMovement.y;
            set => _appliedMovement.y = value;
        }

        public float AppliedMovementX {
            get => _appliedMovement.x;
            set => _appliedMovement.x = value;
        }

        public float AppliedMovementZ {
            get => _appliedMovement.z;
            set => _appliedMovement.z = value;
        }

        public float RunMultiplier {
            get => _runMultiplier;
        }

        public float WalkMultiplier {
            get => _walkMultiplier;
        }

        public CharacterController CharacterController {
            get => _characterController;
        }

        public Dictionary<int, float> JumpGravities {
            get => _jumpGravities;
        }

        public float FallTerminalVelocity {
            get => _fallTerminalVelocity;
        }

        public bool IsMovementPressed {
            get => _isMovementPressed;
        }

        public float Gravity {
            get => _gravity;
        }

        public bool IsSlashing {
            get => _isSlashing;
            set => _isSlashing = value;
        }

        public bool IsRolling {
            get => _isRolling;
            set => _isRolling = value;
        }

        public bool IsBlocking {
            get => _isBlocking;
            set => _isBlocking = value;
        }

        public Weapon Weapon {
            get => _weapon;
        }

        #endregion

        // state variables
        private PlayerBaseState _currentState;
        private PlayerStateFactory _states;

        // constants
        private float _rotationFactorPerFrame = 10.0f;
        private float _runMultiplier = 7.0f;
        private float _walkMultiplier = 3.0f;
        private float _gravity = -9.8f;

        // references and class fields
        private CharacterController _characterController;
        private Animator _animator;

        private FrameInput _input;

        // move fields
        private Vector3 _currentMovement;
        private Vector3 _appliedMovement;
        private Vector3 _cameraRelativeMovement;

        private bool _isMovementPressed;

        // combat fields
        [SerializeField] private Weapon _weapon;
        private int _currentHealth;
        private bool _isSlashing;
        private bool _isRolling;
        private bool _isBlocking;
        private Coroutine _deathRoutine;

        // jump fields
        private bool _isJumping = false;
        private float _initialJumpVelocity;
        private float _maxJumpHeight = 3f;
        private float _maxJumpTime = 0.75f;
        private float _fallTerminalVelocity = -20.0f;
        private int _jumpCount = 0;
        private Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
        private Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
        private Coroutine _currentJumpResetCoroutine;

        private void Awake() {
            // set reference variables
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();

            // setup states
            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded();
            _currentState.EnterState();

            // whatever this is
            SetupJumpVariables();

        }

        private void Start() {
            _currentHealth = Stats.Health;
        }

        private void SetupJumpVariables() {
            float timeToApex = _maxJumpTime / 2;
            float initialGravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
            float secondJumpGravity = (-2 * (_maxJumpHeight + 2)) / MathF.Pow((timeToApex * 1.25f), 2);
            float secondJumpInitialVelocity = (2 * (_maxJumpHeight + 2)) / (timeToApex * 1.25f);
            float thirdJumpGravity = (-2 * (_maxJumpHeight + 4)) / MathF.Pow((timeToApex * 1.5f), 2);
            float thirdJumpInitialVelocity = (2 * (_maxJumpHeight + 4)) / (timeToApex * 1.5f);

            _initialJumpVelocities.Add(1, _initialJumpVelocity);
            _initialJumpVelocities.Add(2, secondJumpInitialVelocity);
            _initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

            _jumpGravities.Add(0, initialGravity);
            _jumpGravities.Add(1, initialGravity);
            _jumpGravities.Add(2, secondJumpGravity);
            _jumpGravities.Add(3, thirdJumpGravity);
        }

        private void HandleRotation() {
            if (!_isMovementPressed || _isSlashing) return;

            // get current rotation and movement
            Quaternion currentRotation = transform.rotation;
            Vector3 positionToLookAt;
            positionToLookAt.x = _cameraRelativeMovement.x;
            positionToLookAt.y = 0.0f;
            positionToLookAt.z = _cameraRelativeMovement.z;

            // make sure we dont pass zero as target
            if (positionToLookAt == Vector3.zero) positionToLookAt = transform.forward;

            // slerp and execute rotation
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation =
                Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
            ;
        }

        private void Update() {
            _currentState.UpdateStates();
            HandleRotation();
            _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
            _characterController.Move(_cameraRelativeMovement * Time.deltaTime);
        }

        //TODO: Create a separate health/Damage script for everyone
        public override void TakeDamage(int dmg) {
            if (_deathRoutine != null) return;

            _animator.Play(PlayerAnimHashes.Damage);
            _currentHealth -= dmg;
            Debug.Log("HEALTH:" + _currentHealth + "/" + Stats.Health);
            if (_currentHealth < 0) {
                _deathRoutine = StartCoroutine(DeathRoutine());
            }
        }

        private IEnumerator DeathRoutine() {
            // start death animation
            _animator.Play(PlayerAnimHashes.Death);
            yield return new WaitForSeconds(3f);
            GameManager.Instance.ChangeState(GameState.Lose);
            yield return new WaitForSeconds(1f);
            UnitManager.Instance.DespawnPlayer(transform.gameObject);
        }

        Vector3 ConvertToCameraSpace(Vector3 vectorToRotate) {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // remove the Y values to ignore upward/down
            cameraForward.y = 0.0f;
            cameraRight.y = 0.0f;

            // re-normalize both vector so they each have a magnitude of 1
            cameraForward = cameraForward.normalized;
            cameraForward = cameraForward.normalized;

            // rotate the X and Z VectorToRotate values to camera space
            Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
            Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

            // the sum of both products is the Vector3 in camera space
            Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
            // set the unchanged y back
            vectorRotatedToCameraSpace.y = vectorToRotate.y;

            return vectorRotatedToCameraSpace;
        }

        #region Gizmos

        private void OnDrawGizmos() {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward * 10f, Color.blue, .1f);
        }

        #endregion

        #region GatherInput

        // handler for movement vars
        public void OnMove(InputAction.CallbackContext context) {
            _input.Movement = context.ReadValue<Vector2>();
            _currentMovement.x = _input.Movement.x;
            _currentMovement.z = _input.Movement.y;
            _isMovementPressed = _input.Movement.x != 0 || _input.Movement.y != 0;
        }
        
        public void OnMoveCamera(InputAction.CallbackContext context) {
            _input.CameraMovement = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context) {
            _input.Attack = context.action.WasPerformedThisFrame();
        }

        public void OnRoll(InputAction.CallbackContext context) {
            _input.Roll = context.action.WasPerformedThisFrame();
        }

        public void OnBlock(InputAction.CallbackContext context) {
            _input.Block = context.action.IsPressed();
        }

        public void OnRun(InputAction.CallbackContext context) =>
            _input.Run = context.action.IsPressed();

        public void OnJump(InputAction.CallbackContext context) {
            _input.Jump = context.action.WasPressedThisFrame();
        }

        #endregion
    }
}