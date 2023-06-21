using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Dzajna {
public class EnemyManager : CharacterManager {
    private EnemyCharacterAnimatorManager enemyCharacterAnimatorM;
    private EnemyStatsManager enemyStatsManager;

    public NavMeshAgent NavMeshAgent;
    public Rigidbody Rigidbody;
    public State CurrentState;
    public CharacterManager CurrentTarget;
    public bool IsPerformingAction;
    public bool IsPhaseShifting;

    //TODO: divide these
    [Header("AI Settings")] //
    public float AggroRange = 10;
    public float MinDetectionAngle = 50;
    public float MaxDetectionAngle = -50;
    public float StoppingDistance = 1.2f;
    public float RotationSpeed = 10f;
    public float MaxAttackRange = 3f;
    public float MaxAggroRadius = 3f;
    public float CurrentRecoveryTime;
    public bool AllowAIToPerformCombos = true;
    public HumanAICombatStyle combatStyle = HumanAICombatStyle.Melee;
    public float ComboChance = 50;
    public bool CanRotate;

    [Header("Settings for Advanced AI")] //
    public bool allowAIToBlock;
    public float blockLikelihood = 30f; // in percentage TODO: .base these on attack speed too
    public bool allowAIToDodge;
    public float dodgeLikelihood = 30f;
    public bool allowAIToParry; // TODO: HAA
    public float parryLikelihood = 30f;

    [Header("A.I Target Information")] //
    public float DistanceFromTarget;
    public Vector3 TargetsDirection;
    public float ViewableAngle;

    protected override void Awake() {
        base.Awake();
        enemyCharacterAnimatorM = GetComponent<EnemyCharacterAnimatorManager>();
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        NavMeshAgent = GetComponentInChildren<NavMeshAgent>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Start() {
        base.Start();
        NavMeshAgent.enabled = false;
        Rigidbody.isKinematic = false;
    }

    private void Update() {
        HandleRecoveryTimer();
        HandleStateMachine();

        IsPhaseShifting = enemyCharacterAnimatorM.Anim.GetBool(enemyCharacterAnimatorM.IsPhaseShiftingHash);
        IsInteracting = enemyCharacterAnimatorM.Anim.GetBool(enemyCharacterAnimatorM.IsInteractingHash);
        CanDoCombo = enemyCharacterAnimatorM.Anim.GetBool(enemyCharacterAnimatorM.CanDoComboHash);
        CanRotate = enemyCharacterAnimatorM.Anim.GetBool(enemyCharacterAnimatorM.CanRotateHash);
        IsRotatingWithRM = enemyCharacterAnimatorM.Anim.GetBool(enemyCharacterAnimatorM.IsRotatingWithRMHash);
        IsInvulnerable = enemyCharacterAnimatorM.Anim.GetBool(enemyCharacterAnimatorM.IsInvulnerableHash);

        enemyCharacterAnimatorM.Anim.SetBool(enemyCharacterAnimatorM.IsDeadHash, IsDead);

        if (CurrentTarget != null) {
            DistanceFromTarget = Vector3.Distance(CurrentTarget.transform.position, transform.position);
            TargetsDirection = CurrentTarget.transform.position - transform.position;
            ViewableAngle = Vector3.Angle(TargetsDirection, transform.forward);
        }
    }

    private void LateUpdate() {
        NavMeshAgent.transform.localPosition = Vector3.zero;
        NavMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine() {
        // Actually handle this error
        if (CurrentState == null) return;
        if (IsDead) return;

        State nextState = CurrentState.Tick(this);
        if (nextState != null) SwitchToNextState(nextState);
    }

    private void SwitchToNextState(State newState) {
        CurrentState = newState;
    }

    private void HandleRecoveryTimer() {
        if (CurrentRecoveryTime > 0) CurrentRecoveryTime -= Time.deltaTime;
        if (IsPerformingAction && CurrentRecoveryTime <= 0) IsPerformingAction = false;
    }
}
}