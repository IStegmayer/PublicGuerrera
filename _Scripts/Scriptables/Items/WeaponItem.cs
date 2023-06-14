using UnityEngine;

namespace Dzajna {
[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item {
    public GameObject ModelPrefab;
    public bool IsUnarmed;

    [Header("Animator Replacer")] //
    public AnimatorOverrideController WeaponController;

    [Header("Weapon Type")] // 
    public WeaponType WeaponType;

    [Header("Damage")] // 
    public int BaseDamage;
    public int CriticalDamageMultiplier;

    [Header("Poise")] //
    public float PoiseBreak;
    public float OffensivePoiseBonus;
    
    [Header("Absorption")] //
    public float PhysicalBlockingDamageAbsorption;
    public float FireBlockingDamageAbsorption;

    [Header("Stability")] //
    public int Stability = 67;
    
    [Header("Stamina Costs")] // 
    public int BaseStamina;
    public float LightAttackMultiplier;
    public float HeavyAttackMultiplier;
    
    [Header("Item Actions")] //
    public ItemAction HoldRBAction;
    public ItemAction TapRBAction;
    public ItemAction TapLBAction;
    public ItemAction HoldLBAction;
    public ItemAction TapRTAction;
    public ItemAction HoldRTAction;
    public ItemAction TapLTAction;
    public ItemAction HoldLTAction;

    [Header("TWO Handed Item Actions")] // 
    public ItemAction TH_HoldRBAction;
    public ItemAction TH_TapRBAction;
    public ItemAction TH_TapLBAction;
    public ItemAction TH_HoldLBAction;
    public ItemAction TH_TapRTAction;
    public ItemAction TH_HoldRTAction;
    public ItemAction TH_TapLTAction;
    public ItemAction TH_HoldLTAction;
    
    [Header("SOUND FX")] // 
    public AudioClip[] WeaponWhooshes;
}
}