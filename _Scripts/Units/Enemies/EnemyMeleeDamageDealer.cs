using System;
using UnityEngine;

public class EnemyMeleeDamageDealer : MonoBehaviour
{
    private BaseUnit enemy;

    //TODO: This should be a class managed by the Unit itself using scriptableObjects
    // but this'll do for now
    [SerializeField] private int weaponDmg = 1;
    private bool canDealDamage;

    public void ToggleEnableDamage(bool state) => canDealDamage = state;

    private void Awake() {
        enemy = transform.root.gameObject.GetComponentInChildren<BasicGoblin>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!canDealDamage) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (!other.gameObject.TryGetComponent(out BaseUnit player)) return;
        
        AudioSystem.Instance.PlaySound("playerDamage", player.transform.position);
        player.TakeDamage(enemy.Stats.BaseAttackPower + weaponDmg);
        canDealDamage = false;
    }
}