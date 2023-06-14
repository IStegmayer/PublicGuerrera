using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "Item Actions/Attempt Critical Attack Action")]
public class CriticalAttackAction : ItemAction {
    public override void PerformAction(CharacterManager character) {
        if (character.IsInteracting) return;

        character.CharacterCombatManager.AttemptBackStabOrRiposte();
    }
}
}