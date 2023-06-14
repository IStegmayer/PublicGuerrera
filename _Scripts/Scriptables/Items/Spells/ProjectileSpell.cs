using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "Spells/ProjectileSpell")]
public class ProjectileSpell : SpellItem {
    public float BaseDamage;

    [Header("Projectile physics")] public float ProjectileForwardVelocity;
    public float ProjectileUpwardVelocity;
    public float ProjectileMass;
    public bool UsesGravity;

    private Rigidbody rigidbody;
    private Quaternion castRotation;

    public override void AttemptToCastSpell(CharacterManager character) {
        base.AttemptToCastSpell(character);
        castRotation = PlayerCamera.Instance.GetCameraRotation();
        GameObject warmUpSFXInstance =
            Instantiate(spellWarmUpFX, character.CharacterWeaponSlotManager.GetRightHandTransform());
        character.CharacterAnimatorManager.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SuccessfullyCastedSpell(CharacterManager character) {
        base.SuccessfullyCastedSpell(character);
        CharacterAnimatorManager charAnim = character.CharacterAnimatorManager;
        PlayerCamera camera = PlayerCamera.Instance;
        GameObject spellFXInstance = Instantiate(spellCastFX,
            character.CharacterWeaponSlotManager.GetRightHandTransform().position,
            castRotation);
        rigidbody = spellFXInstance.GetComponent<Rigidbody>();
        charAnim.Anim.SetBool(charAnim.IsFiringSpellHash, true);
        // spellDamageCollider - spellFXInstance.GetComponent<SpellDamageCollider>();

        if (camera.CurrentLockOnTarget != null)
            spellFXInstance.transform.LookAt(camera.CurrentLockOnTarget.transform);
        else
            spellFXInstance.transform.rotation = Quaternion.Euler(camera.GetCameraTransform().eulerAngles.x,
                character.transform.eulerAngles.y, 0);

        rigidbody.AddForce(spellFXInstance.transform.forward * ProjectileForwardVelocity);
        rigidbody.AddForce(spellFXInstance.transform.up * ProjectileUpwardVelocity);
        rigidbody.useGravity = UsesGravity;
        rigidbody.mass = ProjectileMass;
        spellFXInstance.transform.parent = null;
    }
}
}