using UnityEngine;

namespace Dzajna {
public class CharacterEffectsManager : MonoBehaviour {
    [Header("Damage FX")]
    [SerializeField] private GameObject bloodSplatterFX;
    
    [Header("Weapon FX")]
    public WeaponFX RightWeaponFX;
    public WeaponFX LeftWeaponFX;

    public virtual void PlayRightWeaponFX() {
        if (RightWeaponFX != null) RightWeaponFX.PlayWeaponFX();
    }

    public virtual void PlayLeftWeaponFX() {
        if (LeftWeaponFX != null) LeftWeaponFX.PlayWeaponFX();
    }

    public virtual void StopRightWeaponFX() {
        if (RightWeaponFX != null) RightWeaponFX.StopWeaponFX();
    }

    public virtual void StopLeftWeaponFX() {
        if (LeftWeaponFX != null) LeftWeaponFX.StopWeaponFX();
    }

    public virtual void PlayBloodSplatterFX(Vector3 _bloodSplatterPos) {
        GameObject blood = Instantiate(bloodSplatterFX, _bloodSplatterPos, Quaternion.identity);
    }
}
}