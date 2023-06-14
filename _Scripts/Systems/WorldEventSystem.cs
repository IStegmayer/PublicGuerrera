using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {

// TODO: this is just GameManager tbh
public class WorldEventSystem : MonoBehaviour {
    // Fog Wall
    [SerializeField] private BossHealthBar bossHealthBar;
    [SerializeField] private List<FogWall> fogWalls;
    [SerializeField] private EnemyBossManager boss;
    
    public bool BossFightIsActive; // Is currently fighting boss
    public bool BossHasBeenAwakened; // Woke the boss/watched cutscene but died during fight
    public bool BossHasBeenDefeated; // Boss has been defeated

    public void ActivateBossFight() {
        BossFightIsActive = true;
        BossHasBeenAwakened = true;
        bossHealthBar.ToggleBossHealthBarActivation(true);
        foreach (var fogWall in fogWalls)
            fogWall.ToggleFogWallActivation(true);
    }

    public void BossDefeated() {
        BossHasBeenDefeated = true;
        BossFightIsActive = false;
        foreach (var fogWall in fogWalls)
            fogWall.ToggleFogWallActivation(false);
    }
}
}
