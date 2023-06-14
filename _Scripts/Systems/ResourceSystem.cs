using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSystem : StaticInstance<ResourceSystem>
{
    private ScriptablePlayer _baseScriptablePlayer;
    public List<ScriptableEnemy> Enemies { get; private set; }
    private Dictionary<EnemyType, ScriptableEnemy> _EnemiesDict;

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources() {
        Enemies = Resources.LoadAll<ScriptableEnemy>("Enemies").ToList();
        _EnemiesDict = Enemies.ToDictionary(e => e.EnemyType, e => e);
        _baseScriptablePlayer = Resources.Load<ScriptablePlayer>("Player");
    }

    public ScriptableEnemy GetEnemy(EnemyType t) => _EnemiesDict[t];

    public ScriptablePlayer GetPlayer(int level) {
        var playerInstance = _baseScriptablePlayer;
        playerInstance.PlayerLevel = level;
        return playerInstance;
    }
}