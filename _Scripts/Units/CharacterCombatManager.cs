using System;
using System.Collections;
using UnityEngine;

namespace Dzajna {
public class CharacterCombatManager : MonoBehaviour {
    [SerializeField] protected Transform critRayCastStartPoint;
    [SerializeField] private Transform backStabberStandPoint;
    [SerializeField] private Transform riposterStandPoint;
    [SerializeField] protected float maxCritDistance = 1.5f;
    [SerializeField] private LayerMask characterLayer;
    public int LastAttackHash;
    public AttackType CurrentAttackType;

    protected CharacterManager character;

    protected virtual void Start() {
        character = GetComponent<CharacterManager>();
    }

    public void SetBlockingAbsorptionsFromBlockingWeapon() {
        CharacterStatsManager charStats = character.CharacterStatsManager;
        CharacterInventoryManager charInventory = character.CharacterInventoryManager;

        if (character.IsUsingRightHand) {
            charStats.BlockingPhysicalDamageAbsorption = charInventory.RightWeapon.PhysicalBlockingDamageAbsorption;
            charStats.BlockingFireDamageAbsorption = charInventory.RightWeapon.FireBlockingDamageAbsorption;
            charStats.BlockingStabilityRating = charInventory.RightWeapon.Stability;
        }
        else if (character.IsUsingLeftHand) {
            charStats.BlockingPhysicalDamageAbsorption = charInventory.LeftWeapon.PhysicalBlockingDamageAbsorption;
            charStats.BlockingFireDamageAbsorption = charInventory.LeftWeapon.FireBlockingDamageAbsorption;
            charStats.BlockingStabilityRating = charInventory.LeftWeapon.Stability;
        }
    }

    public void AttemptBackStabOrRiposte() {
        if (character.IsInteracting) return;
        if (!character.CharacterStatsManager.CheckHasStamina()) return;

        RaycastHit hit;

        if (!Physics.Raycast(critRayCastStartPoint.transform.position,
                critRayCastStartPoint.transform.forward, out hit, maxCritDistance, characterLayer))
            return;

        var enemyCharacter = hit.transform.GetComponent<CharacterManager>();
        var directionFromCharacterToEnemy =
            critRayCastStartPoint.transform.position - enemyCharacter.transform.position;
        var dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward);

        Debug.Log("CURRENT DOT VALUE IS " + dotValue);

        if (enemyCharacter.CanBeRiposted)
            if (dotValue <= 1.2f && dotValue >= 0.6f) {
                AttemptRiposte(hit);
                return;
            }

        if (dotValue >= -0.8f && dotValue <= -0.4f) AttemptBackStab(hit);
    }

    private void AttemptBackStab(RaycastHit hit) {
        var enemyCharacter = hit.transform.GetComponent<CharacterManager>();

        if (enemyCharacter == null) return;
        if (enemyCharacter.IsBeingBackstabbed && enemyCharacter.IsBeingRiposted) return;

        var charAnim = character.CharacterAnimatorManager;
        var charInv = character.CharacterInventoryManager;

        //We make it so the enemy cannot be damaged whilst being critically damaged
        charAnim.EnableIsInvulnerable();
        // charAnim.EraseHandIKForWeapon();
        character.IsPerformingBackstab = true;

        charAnim.PlayTargetAnimation(charAnim.BackStabHash, true);

        float criticalDamage = charInv.RightWeapon.CriticalDamageMultiplier * charInv.RightWeapon.BaseDamage;
        enemyCharacter.PendingCriticalDamage = criticalDamage;
        enemyCharacter.CharacterCombatManager.GetBackStabbed(character, enemyCharacter.CharacterAnimatorManager);
    }

    private void AttemptRiposte(RaycastHit hit) {
        var enemyCharacter = hit.transform.GetComponent<CharacterManager>();

        if (enemyCharacter == null) return;
        if (enemyCharacter.IsBeingBackstabbed && enemyCharacter.IsBeingRiposted) return;

        var charAnim = character.CharacterAnimatorManager;
        var charInv = character.CharacterInventoryManager;

        //We make it so the enemy cannot be damaged whilst being critically damaged
        charAnim.EnableIsInvulnerable();
        // charAnim.EraseHandIKForWeapon();
        character.IsPerformingRiposte = true;

        charAnim.PlayTargetAnimation(charAnim.RiposteHash, true);

        float criticalDamage = charInv.RightWeapon.CriticalDamageMultiplier * charInv.RightWeapon.BaseDamage;
        enemyCharacter.PendingCriticalDamage = criticalDamage;
        enemyCharacter.CharacterCombatManager.GetRiposted(character, enemyCharacter.CharacterAnimatorManager);
    }

    private void GetBackStabbed(CharacterManager characterPerformingBackStab, CharacterAnimatorManager charAnim) {
        //PLAY SOUND FX
        character.IsBeingBackstabbed = true;

        StartCoroutine(ForceMoveCharacterToCriticalPosition(characterPerformingBackStab, true));

        charAnim.PlayTargetAnimation(charAnim.BackStabbedHash, true);
    }

    private void GetRiposted(CharacterManager characterPerformingBackStab, CharacterAnimatorManager charAnim) {
        //PLAY SOUND FX
        character.IsBeingRiposted = true;

        StartCoroutine(ForceMoveCharacterToCriticalPosition(characterPerformingBackStab, false));

        charAnim.PlayTargetAnimation(charAnim.RipostedHash, true);
    }

    private IEnumerator ForceMoveCharacterToCriticalPosition(CharacterManager characterPerformingBackStab,
        bool isBackstab) {
        Transform spotTransform;
        Vector3 fwVector3;
        if (isBackstab) {
            spotTransform = backStabberStandPoint;
            fwVector3 = characterPerformingBackStab.transform.forward;
        }
        else {
            spotTransform = riposterStandPoint;
            fwVector3 = -characterPerformingBackStab.transform.forward;
        }

        for (var timer = 0.05f; timer < 0.5f; timer = timer + 0.05f) {
            var backstabRotation = Quaternion.LookRotation(fwVector3);
            transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
            transform.parent = spotTransform;
            transform.localPosition = spotTransform.localPosition;
            transform.parent = null;
            Debug.Log("Running corountine");
            yield return new WaitForSeconds(0.05f);
        }
    }


    public void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, string block) {
        var staminaDamageAbsorption = (physicalDamage + fireDamage)
                                      * attackingWeapon.GuardBreakModifier
                                      * (character.CharacterStatsManager.BlockingStabilityRating / 100);

        Debug.Log(staminaDamageAbsorption);

        var staminaDamage = (physicalDamage + fireDamage)
            * attackingWeapon.GuardBreakModifier - staminaDamageAbsorption;

        character.CharacterStatsManager.TakeStaminaDamage(staminaDamage);

        if (character.CharacterStatsManager.CheckHasStamina()) {
            character.IsBlocking = false;
            character.CharacterAnimatorManager.PlayTargetAnimation("Guard_Break_01", true);
        }
        else {
            character.CharacterAnimatorManager.PlayTargetAnimation(character.CharacterAnimatorManager.BlockHash, true);
        }
    }
}
}