using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "Item Actions/Cast Magic Spell Action")]
public class MagicSpellAction : ItemAction {
    public override void PerformAction(CharacterManager character) {
        if (character.IsInteracting) return;
        CharacterInventoryManager inventory = character.CharacterInventoryManager;
        CharacterAnimatorManager animator = character.CharacterAnimatorManager;

        if (inventory.CurrentSpell != null &&
            inventory.CurrentSpell.isMagicSpell) {
            if (character.CharacterStatsManager.CheckHasEnoughFP(inventory.CurrentSpell.FPCost))
                inventory.CurrentSpell.AttemptToCastSpell(character);
            else
                animator.PlayTargetAnimation(animator.FailedCastHash, true);
        }
    }
}
}