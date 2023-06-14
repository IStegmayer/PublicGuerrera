using System;
using UnityEngine;

namespace Dzajna
{
    public class EnemyCharacterAnimatorManager : CharacterAnimatorManager
    {
        private Rigidbody rigidbody;
        private EnemyStatsManager enemyStatsManager;
        private EnemyManager enemyManager;

        
        protected override void Awake() {
            base.Awake();
            Anim = GetComponent<Animator>();
            rigidbody = GetComponentInChildren<Rigidbody>();
            enemyManager = GetComponent<EnemyManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
        }

        protected override void OnAnimatorMove() {
            Vector3 deltaPosition = Anim.deltaPosition;
            characterManager.CharacterController.Move(deltaPosition);
            characterManager.transform.rotation *= Anim.deltaRotation;
        
            if (enemyManager.IsRotatingWithRM) enemyManager.transform.rotation *= Anim.deltaRotation;
        }
        
        //TODO: this is not the best to communicate between scripts for sure, use unitmanager instead! 
        public void AwardSoulsOnDeath() {
            PlayerStatsManager playerStatsManager = FindObjectOfType<PlayerStatsManager>();
            SoulCountUI _soulCount = FindObjectOfType<SoulCountUI>();
            if (playerStatsManager != null) playerStatsManager.AddSouls(enemyStatsManager.SoulsReward);
            if (_soulCount != null) _soulCount.SetSoulCountText(playerStatsManager.SoulCount);
        }
        public override void TakeCriticalDamageAnimEvent() {
            enemyStatsManager.TakeDamageWithoutAnimation(enemyManager.PendingCriticalDamage);
            enemyManager.PendingCriticalDamage = 0;
        }
    }
}