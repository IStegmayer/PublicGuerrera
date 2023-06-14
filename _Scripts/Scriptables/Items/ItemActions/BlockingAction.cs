using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "Item Actions/Blocking Action")]
public class BlockingAction : ItemAction {
    public override void PerformAction(CharacterManager character) {
        if (character.IsInteracting || character.IsBlocking) return;

        character.CharacterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
        character.IsBlocking = true;
    }
}
}