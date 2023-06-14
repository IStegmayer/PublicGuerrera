using UnityEngine;

namespace Dzajna {
public class WeaponFX : MonoBehaviour {
    [Header("Weapon FX")] [SerializeField] private ParticleSystem normalWeaponTrail;

    public void PlayWeaponFX() => normalWeaponTrail.Play();
    public void StopWeaponFX() => normalWeaponTrail.Stop();
}
}