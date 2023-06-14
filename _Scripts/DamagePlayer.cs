using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Dzajna
{
    public class DamagePlayer : MonoBehaviour
    {
        [SerializeField] private int damage = 10;

        private void OnTriggerEnter(Collider _other) {
            PlayerStatsManager playerStatsManager = _other.GetComponent<PlayerStatsManager>();
            
            if (playerStatsManager != null) playerStatsManager.TakeDamage(damage);
        }
    }
}
