using System;
using UnityEngine;

/// <summary>
// Create a scriptable enemy with given type 
/// </summary>
[CreateAssetMenu(fileName = "New Scriptable Enemy")]
public class ScriptableEnemy : ScriptableUnitBase {
    public EnemyType EnemyType;
}

[Serializable]
public enum EnemyType {
    Goblin = 0,
    Shaman = 1,
    BossGoblin = 2
}