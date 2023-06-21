using System;
using UnityEngine;

namespace Dzajna {
public class SpellDamageCollider : DamageCollider {
    public GameObject ImpactParticles;
    public GameObject ProjectileParticles;
    public GameObject MuzzleParticles;

    private bool hasCollided;
    private CharacterStatsManager spellTarget;
    private Rigidbody rigidbody;
    private Vector3 impactNormal; //Used to rotate impact particles

    private void Start() {
        ProjectileParticles = Instantiate(ProjectileParticles, transform.position, transform.rotation);
        ProjectileParticles.transform.parent = transform;

        if (MuzzleParticles) {
            Instantiate(MuzzleParticles, transform.position, transform.rotation);
            // Destroy(MuzzleParticles, 2f);
        }
    }

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        if (hasCollided) return;

        spellTarget = other.transform.GetComponent<CharacterStatsManager>();
        if (spellTarget != null) spellTarget.TakeDamage(PhysicalDamage); 
        
        hasCollided = true;
        ImpactParticles = Instantiate(ImpactParticles, transform.position,
            Quaternion.FromToRotation(Vector3.up, impactNormal));
        
        Destroy(ProjectileParticles);
        Destroy(ImpactParticles, 1f);
        Destroy(gameObject, 1f);
    }
}
}