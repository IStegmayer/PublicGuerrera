using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {
public class PassThroughFogWall : Interactable {
    private WorldEventSystem worldEventSystem;

    private void Awake() {
        worldEventSystem = FindObjectOfType<WorldEventSystem>();
    }

    public override void Interact(PlayerManager _playerManager) {
        base.Interact(_playerManager);
        _playerManager.PassThroughFogWallInteraction(transform);
    }
}
}
