using System.Collections;
using System.Collections.Generic;
using Game._Scripts;
using UnityEngine;

// Class made to check Shield colliding
public class Shield : MonoBehaviour
{
    private PlayerStateMachine _player;
    private void Awake() {
        _player = transform.root.gameObject.GetComponentInChildren<PlayerStateMachine>();
    }
    
    // private void OnCollisionEnter(Collision other) {
    //     if (!_player.IsBlocking) return;
    //     
    //     // TODO: not the best layermask implementation
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
    //         BaseUnit enemy = other.gameObject.GetComponent<BaseUnit>();
    //         enemy.TakeDamage(_unit.Stats.BaseAttackPower + _weaponDmg);
    //     }
    // }
}
