using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "Item Actions/Light Attack Action")]
public class LightAttackAction : ItemAction {
    public override void PerformAction(CharacterManager character) {
        if (!character.CharacterStatsManager.CheckHasStamina()) return;

        character.IsAttacking = true;
        // character.characterAnimatorManager.EraseHandIKForWeapon();
        character.CharacterEffectsManager.PlayRightWeaponFX();

        // Check for running attack
        if (character.IsSprinting) {
            HandleRunningAttack(character);
            return;
        }

        //Check for combo or first attack. If we're interacting, return
        if (character.CanDoCombo) HandleLightWeaponCombo(character);
        else if (!character.IsInteracting) HandleLightAttack(character);
        else return;

        // update currentAttackType
        character.CharacterCombatManager.CurrentAttackType = AttackType.Light;
    }

    //TODO: clean these up, too many unnecessary if elses
    private void HandleRunningAttack(CharacterManager character) {
        CharacterAnimatorManager charAnim = character.CharacterAnimatorManager;
        CharacterCombatManager combatManager = character.CharacterCombatManager;

        if (character.IsUsingLeftHand) {
            charAnim.PlayTargetAnimation(charAnim.RunningAttackHash, true, false, true);
            combatManager.LastAttackHash = charAnim.RunningAttackHash;
        }
        else if (character.IsUsingRightHand) {
            if (character.IsTwoHandingWeapon) {
                charAnim.PlayTargetAnimation(charAnim.TH_RunningAttackHash, true);
                combatManager.LastAttackHash = charAnim.TH_RunningAttackHash;
            }
            else {
                charAnim.PlayTargetAnimation(charAnim.RunningAttackHash, true);
                combatManager.LastAttackHash = charAnim.RunningAttackHash;
            }
        }
    }

    private void HandleLightAttack(CharacterManager character) {
        CharacterAnimatorManager charAnim = character.CharacterAnimatorManager;

        if (character.IsUsingLeftHand) {
            charAnim.PlayTargetAnimation(charAnim.LightAttack1Hash, true, false, true);
            character.CharacterCombatManager.LastAttackHash = charAnim.LightAttack1Hash;
        }
        else if (character.IsUsingRightHand) {
            if (character.IsTwoHandingWeapon) {
                charAnim.PlayTargetAnimation(charAnim.TH_LightAttack1Hash, true);
                character.CharacterCombatManager.LastAttackHash = charAnim.TH_LightAttack1Hash;
            }
            else {
                charAnim.PlayTargetAnimation(charAnim.LightAttack1Hash, true);
                character.CharacterCombatManager.LastAttackHash = charAnim.LightAttack1Hash;
            }
        }
    }

    private void HandleLightWeaponCombo(CharacterManager character) {
        CharacterAnimatorManager charAnim = character.CharacterAnimatorManager;
        CharacterCombatManager combatManager = character.CharacterCombatManager;

        character.animator.SetBool(charAnim.CanDoComboHash, false);

        if (character.IsUsingLeftHand) {
            if (combatManager.LastAttackHash == charAnim.LightAttack1Hash) {
                charAnim.PlayTargetAnimation(charAnim.LightAttack2Hash, true, false, true);
                combatManager.LastAttackHash = charAnim.LightAttack2Hash;
            }
            else {
                charAnim.PlayTargetAnimation(charAnim.LightAttack1Hash, true, false, true);
                combatManager.LastAttackHash = charAnim.TH_LightAttack1Hash;
            }
        }
        else if (character.IsUsingRightHand) {
            if (character.IsTwoHandingWeapon) {
                if (combatManager.LastAttackHash == charAnim.TH_LightAttack1Hash) {
                    charAnim.PlayTargetAnimation(charAnim.TH_LightAttack2Hash, true);
                    combatManager.LastAttackHash = charAnim.TH_LightAttack2Hash;
                }
                else {
                    charAnim.PlayTargetAnimation(charAnim.TH_LightAttack1Hash, true);
                    combatManager.LastAttackHash = charAnim.TH_LightAttack1Hash;
                }
            }
            else {
                if (combatManager.LastAttackHash == charAnim.LightAttack1Hash) {
                    charAnim.PlayTargetAnimation(charAnim.LightAttack2Hash, true);
                    combatManager.LastAttackHash = charAnim.LightAttack2Hash;
                }
                else {
                    charAnim.PlayTargetAnimation(charAnim.LightAttack1Hash, true);
                    combatManager.LastAttackHash = charAnim.TH_LightAttack1Hash;
                }
            }
        }
    }
}
}