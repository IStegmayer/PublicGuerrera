using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {
public class PlayerCamera : PersistentSingleton<PlayerCamera> {
    [Header("References")] 
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraPivotTransform;
    [SerializeField] PlayerManager playerManager;
    private Transform playerTransform;

    [Header("Camera Settings")] 
    [SerializeField] private float minPivot = -30;
    [SerializeField] private float maxPivot = 60;
    [SerializeField] private float leftAndRightRotationSpeed = 220;
    [SerializeField] private float upAndDownRotationSpeed = 220;
    [SerializeField] private float cameraSmoothSpeed = 1; // Bigger number means more smoothing. Change depending on different movesets
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask collideWithLayers;

    [Header("Camera Values")] 
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;
    private float defaultCameraZPosition; // Values used for camera collisions
    private float targetCameraZPosition; // Values used for camera collisions
    private Vector3 cameraObjectPosition; // Used for camera collisions (moves the camera to this pos upon colliding)
    private Vector3 cameraVelocity;
    
    [Header("Lock on")] 
    [SerializeField] private float maximumLockOnDistance = 30f;
    [SerializeField] private float lockedPivotPosition = 2.25f;
    [SerializeField] private float unlockedPivotPosition = 1.65f;
    public CharacterManager CurrentLockOnTarget;
    private CharacterManager nearestLockOnTarget;
    private CharacterManager leftLockOnTarget;
    private CharacterManager rightLockOnTarget;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();

    protected override void Awake() {
        base.Awake();
    }

    private void Start() {
        defaultCameraZPosition = cameraTransform.localPosition.z;
        playerTransform = playerManager.transform;
    }

    private void Update() {
        HandleAllCameraActions();
    }

    private void HandleAllCameraActions() {
        if (playerManager == null) return;
        HandleFollowTarget();
        HandleRotation();
        HandleCollisions();
    }

    private void HandleFollowTarget() {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, playerManager.transform.position,
            ref cameraVelocity, 0.1f * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotation() {
        float delta = Time.deltaTime;
        SetCameraHeight(delta);
        
        // IF LOCKED ON, FORCE ROTATION TOWARDS TARGET
        if (!InputHandler.Instance.LockOnFlag && CurrentLockOnTarget == null) {
            // Rotate based on horizontal/vertical movement of the joystick
            leftAndRightLookAngle += (InputHandler.Instance.CameraX * leftAndRightRotationSpeed) * delta;
            upAndDownLookAngle -= (InputHandler.Instance.CameraY * upAndDownRotationSpeed) * delta;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minPivot, maxPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // Rotate this gameObject left/right
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = Quaternion.Slerp
                (transform.rotation, targetRotation, cameraSmoothSpeed * delta);
            // Rotate the pivot up/right
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = Quaternion.Slerp
                (cameraPivotTransform.localRotation, targetRotation, cameraSmoothSpeed * delta);
        }
        else {
            float velocity = 0;
            Vector3 targetPos = CurrentLockOnTarget.transform.position;

            Vector3 dir = targetPos - transform.position;
            dir.y = 0;
            dir.Normalize();

            //TODO: Slerp both of these?
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp
                (transform.rotation, targetRotation, cameraSmoothSpeed * delta);

            dir = targetPos - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
    }

    public void HandleLockOn() {
        Vector3 playerTargetPosition = playerTransform.position;
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(playerTargetPosition, 26);

        for (int i = 0; i < colliders.Length; i++) {
            EnemyManager character = colliders[i].GetComponent<EnemyManager>();
            if (character == null) continue;

            Vector3 lockTargetDirection = character.transform.position - playerTargetPosition;
            float distanceFromTarget = Vector3.Distance(playerTargetPosition, character.transform.position);
            float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
            RaycastHit hit;

            if (character.transform != playerTransform.transform && viewableAngle > -50 &&
                viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance) {
                if (Physics.Linecast(playerManager.LockOnTransform.position, character.LockOnTransform.position,
                        out hit, collideWithLayers)) {
                    Debug.DrawLine(playerManager.LockOnTransform.position, character.LockOnTransform.position);
                    availableTargets.Add(character);
                }
            }
        }

        for (int j = 0; j < availableTargets.Count; j++) {
            Vector3 targetPos = availableTargets[j].transform.position;

            float distanceFromTarget =
                Vector3.Distance(playerTargetPosition, targetPos);
            if (distanceFromTarget < shortestDistance) {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[j];
            }

            if (!InputHandler.Instance.LockOnFlag) return;

            Vector3 relativeEnemyPosition = playerManager.transform.InverseTransformPoint(targetPos);
            float distanceFromLeftTarget = relativeEnemyPosition.x;
            float distanceFromRightTarget = relativeEnemyPosition.x;

            if (relativeEnemyPosition.x <= 0.0f && distanceFromLeftTarget > shortestDistanceOfLeftTarget &&
                availableTargets[j] != CurrentLockOnTarget) {
                shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                leftLockOnTarget = availableTargets[j];
            }
            else if (relativeEnemyPosition.x > 0.0f && distanceFromRightTarget < shortestDistanceOfRightTarget &&
                     availableTargets[j] != CurrentLockOnTarget) {
                shortestDistanceOfRightTarget = distanceFromRightTarget;
                rightLockOnTarget = availableTargets[j];
            }
        }
    }

    public bool SetCurrentLockOnTarget() {
        if (nearestLockOnTarget == null) return false;

        CurrentLockOnTarget = nearestLockOnTarget;
        return true;
    }

    public void SwitchLockOnTarget(bool leftInput, bool rightInput) {
        if (leftInput && leftLockOnTarget != null) CurrentLockOnTarget = leftLockOnTarget;
        if (rightInput && rightLockOnTarget != null) CurrentLockOnTarget = rightLockOnTarget;
    }

    public void ClearLockOnTargets() {
        availableTargets.Clear();
        nearestLockOnTarget = null;
        CurrentLockOnTarget = null;
        leftLockOnTarget = null;
        rightLockOnTarget = null;
    }    
    private void SetCameraHeight(float delta) {
        Vector3 velocity = Vector3.zero;
        float newPivotY = CurrentLockOnTarget != null ? lockedPivotPosition : unlockedPivotPosition;
        Vector3 newPivotPosition = new Vector3(0, newPivotY);

        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
            cameraPivotTransform.transform.localPosition,
            newPivotPosition, ref velocity, (cameraSmoothSpeed / 2) * delta);
    }

    private void HandleCollisions() {
        targetCameraZPosition = defaultCameraZPosition;
        RaycastHit hit;
        Vector3 rayDirection = cameraTransform.position - cameraPivotTransform.position;
        rayDirection.Normalize();

        // Check for collision with environment
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, rayDirection, out hit,
                Mathf.Abs(targetCameraZPosition), collideWithLayers)) {
            // Get distance from collision object
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        // Apply our offset to maintain radius distance (snap back)
        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            targetCameraZPosition = -cameraCollisionRadius;

        // Then we lerp to our new position
        cameraObjectPosition.z =
            Mathf.Lerp(cameraTransform.localPosition.z, targetCameraZPosition,
                cameraSmoothSpeed * Time.deltaTime);
        cameraObjectPosition.y = 0f;
        cameraTransform.localPosition = cameraObjectPosition;
    }
    
    public Quaternion GetCameraRotation() => cameraPivotTransform.rotation;
    public Transform GetCameraTransform() => cameraPivotTransform;
}
}