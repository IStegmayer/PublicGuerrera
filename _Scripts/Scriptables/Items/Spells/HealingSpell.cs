using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem {
    public int HealAmount;

    public override void AttemptToCastSpell(CharacterManager character) {
        base.AttemptToCastSpell(character);
        // GameObject warmUpSpellFXInstance = Instantiate(spellWarmUpFX, CharacterAnimatorManager.transform);
        character.CharacterAnimatorManager.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SuccessfullyCastedSpell(CharacterManager character) {
        base.SuccessfullyCastedSpell(character);
        // GameObject spellFXInstance = Instantiate(spellCastFX, CharacterAnimatorManager.transform);
        // CharacterAnimatorManager.PlayTargetAnimation(spellAnimation, true);
        character.CharacterStatsManager.TakeDamage(-HealAmount);
    }
}
}