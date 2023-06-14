using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicGoblin : BaseUnit
{
    [Header("Combat")] [SerializeField] private EnemyMeleeDamageDealer _damageDealer;
    [SerializeField] private float _attackRange = 1.6f;
    [SerializeField] private float _aggroRange = 6f;
    [SerializeField] private float _attackCd = 2f;
    [SerializeField] private float _reaggroCd = 0.5f;
    public Transform LockOnTransform;

    //TODO: this could be further abstracted
    public static event Action<BasicGoblin> GoblinSpawned;
    public static event Action<BasicGoblin> GoblinDamaged;
    public static event Action<BasicGoblin> GoblinKilled;


    private int _currentHealth;
    private Transform _playerTransform;
    private Coroutine _deathRoutine;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private float _attackTimer;

    private void Start() {
        _currentHealth = Stats.Health;
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update() {
        if (_playerTransform == null) return;

        // attack 
        if (_attackTimer >= _attackCd) {
            if (Vector3.Distance(_playerTransform.position, transform.position) <= _attackRange) {
                _damageDealer.ToggleEnableDamage(true);
                _animator.SetTrigger(GoblinAnimHashes.Attack);
                _attackTimer = 0;
            }
        }

        _attackTimer += Time.deltaTime;

        // aggro
        if (_reaggroCd <= 0 && Vector3.Distance(_playerTransform.position, transform.position) <= _aggroRange) {
            _reaggroCd = 0.5f;
            _navMeshAgent.destination = _playerTransform.position;
            _animator.SetBool(GoblinAnimHashes.IsMoving, true);
        }
        else
            _animator.SetBool(GoblinAnimHashes.IsMoving, false);

        _reaggroCd -= Time.deltaTime;
        
        Vector3 playerPosition = _playerTransform.position;
        playerPosition.y = 0;
        transform.LookAt(playerPosition);
    }

    //TODO: Create a separate health/Damage script for everyone
    public override void TakeDamage(int dmg) {
        if (_deathRoutine != null) return;
        Debug.Log("HEALTH:" + _currentHealth + "/" + Stats.Health);

        _animator.Play(GoblinAnimHashes.Damage);
        _currentHealth -= dmg;
        if (_currentHealth < 0) {
            _deathRoutine = StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DeathRoutine() {
        // animate death and invoke event
        _animator.Play(GoblinAnimHashes.Death);
        _navMeshAgent.isStopped = true;
        GoblinKilled?.Invoke(this);
        yield return new WaitForSeconds(4f);
        UnitManager.Instance.DespawnEnemy(this.gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _aggroRange);
    }
}