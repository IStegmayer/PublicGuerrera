using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {
public class EventColliderStartBossFight : MonoBehaviour {
    [SerializeField] private WorldEventSystem worldEventManager;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) worldEventManager.ActivateBossFight();
    }
}
}