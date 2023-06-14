using System.Collections.Generic;
using Dzajna;
using UnityEngine;

public class UnitManager : StaticInstance<UnitManager> {
    [SerializeField] private PlayerCamera playerCamera;
    private Dictionary<int, CharacterManager> spawnedEnemies = new Dictionary<int, CharacterManager>();
    private int enemyCount;
    private BaseUnit playerInstance;

    public Transform SpawnPlayer(Transform transform, int level = 1) {
        ScriptableUnitBase playerScriptable = ResourceSystem.Instance.GetPlayer(level);
        GameObject spawnedGO = SpawnUnit(playerScriptable, transform.position, transform.rotation);
        // BaseUnit player = spawnedGO.GetComponentInChildren<BaseUnit>();
        // player.SetStats(playerScriptable.BaseStats);

        return spawnedGO.transform;
    }

    public void SpawnEnemy(Transform transform, EnemyType type) {
        ScriptableUnitBase enemyScriptable = ResourceSystem.Instance.GetEnemy(type);
        GameObject spawnedGO = SpawnUnit(enemyScriptable, transform.position, transform.rotation);

        CharacterManager enemy = spawnedGO.GetComponentInChildren<CharacterManager>();
        spawnedEnemies.Add(enemyCount, enemy);
        enemyCount++;
    }

    private GameObject SpawnUnit(ScriptableUnitBase unitScriptable, Vector3 pos, Quaternion rotation) {
        //TODO: STATS SHOULD BE SET HERE
        GameObject spawned = Instantiate(unitScriptable.Prefab, pos, rotation, transform);

        return spawned;
    }

    public void DespawnPlayer(GameObject obj) {
        playerInstance = null;
        Destroy(obj);
    }

    // public void DespawnEnemy(int id, GameObject obj) {
    public void DespawnEnemy(GameObject obj) {
        // spawnedEnemies.Remove(id);
        Destroy(obj);
        if (spawnedEnemies.Count == 0) GameManager.Instance.ChangeState(GameState.Win);
    }
}