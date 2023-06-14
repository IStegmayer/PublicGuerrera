using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "Item Actions/Parry Action")]
public class ParryAction : ItemAction {
    public override void PerformAction(CharacterManager character) {
        if (character.IsInteracting) return;
        CharacterAnimatorManager charAnim = character.CharacterAnimatorManager;

        // charAnim.EraseHandIKForWeapon();
        charAnim.PlayTargetAnimation(charAnim.ParryHash, true);

        //TODO: implement different shields
        // //CHECK IF PARRYING WEAPON IS A FAST PARRY WEAPON OR A MEDIUM PARRY WEAPON
        // var parryingWeapon = character.CharacterInventoryManager.CurrentItemBeingUsed as WeaponItem;
        // if (parryingWeapon.WeaponType == WeaponType.SmallShield)
        //     //FAST PARRY ANIM
        //     character.CharacterAnimatorManager.PlayTargetAnimation("Parry", true);
        // else if (parryingWeapon.WeaponType != WeaponType.Shield)
        //     //NORMAL PARRY ANIM
        //     character.CharacterAnimatorManager.PlayTargetAnimation("Parry", true);
    }
}
}