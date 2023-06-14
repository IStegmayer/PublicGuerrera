using UnityEngine;

namespace Dzajna {
public class EnemyBossManager : MonoBehaviour {
    [SerializeField] private string bossName;
    [SerializeField] private GameObject phaseTransitionFX;
    private BossHealthBar bossHealthBar;
    private BossCombatStanceState bossCombatStanceState;
    private EnemyCharacterAnimatorManager enemyCharacterAnimatorManager;
    private EnemyStatsManager enemyStatsManager;
    private EnemyManager enemyManager;

    private void Awake() {
        bossHealthBar = FindObjectOfType<BossHealthBar>(true);
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        enemyManager = GetComponent<EnemyManager>();
        enemyCharacterAnimatorManager = GetComponent<EnemyCharacterAnimatorManager>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start() {
        bossHealthBar.ToggleBossHealthBarActivation(true);
        bossHealthBar.SetBossName(bossName);
        bossHealthBar.SetBossMaxHealth(enemyStatsManager.GetMaxHealth());
    }

    public void UpdateBossHealthBar(float _currentHealth, float _maxHealth) {
        bossHealthBar.SetBossCurrentHealth(_currentHealth);
        if (!bossCombatStanceState.HasPhaseShifted && _currentHealth <= _maxHealth / 2) {
            bossCombatStanceState.HasPhaseShifted = true;
            TransitionToSecondPhase();
        }
    }

    private void TransitionToSecondPhase() {
        enemyManager.IsPhaseShifting = true;
        enemyCharacterAnimatorManager.Anim.SetBool(enemyCharacterAnimatorManager.IsPhaseShiftingHash, true);
        enemyCharacterAnimatorManager.Anim.SetBool(enemyCharacterAnimatorManager.IsInvulnerableHash, true);
        // Play animation w/ an event that trigger particle/weapon fx
        enemyCharacterAnimatorManager.PlayTargetAnimation(enemyCharacterAnimatorManager.PhaseTransitionHash, true);
        // Switch attack actions
    }

    public void InstantiateBossParticleFX() {
        BossFXTransform _bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject _phaseFX = Instantiate(phaseTransitionFX, _bossFXTransform.transform);
    }

    // public void 
    //HANDLE SWITCHING PHASE 
    //HANDLE SWITCHING ATTACK PATTERNS
}
}