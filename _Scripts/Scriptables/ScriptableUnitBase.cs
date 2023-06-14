using System;
using UnityEngine;

/// <summary>
/// Keeping all relevant information about a unit on a scriptable means we can gather and show
/// info on the menu screen, without instantiating the unit prefab.
/// </summary>
public abstract class ScriptableUnitBase : ScriptableObject {

    [SerializeField] private Stats _stats;
    public Stats BaseStats => _stats;

    // Get base class for all units
    public GameObject Prefab;
    
    // Any other info about the unit
}

/// <summary>
/// Keeping base stats as a struct on the scriptable keeps it flexible and easily editable.
/// We can pass this struct to the spawned prefab unit and alter them depending on conditions.
/// </summary>
[Serializable]
public struct Stats {
    public int Health;
    public int BaseAttackPower;
    public float BaseSpeed;
}
