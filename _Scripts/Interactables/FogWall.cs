using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {
public class FogWall : MonoBehaviour {
    private Collider collider;
    
    private void Awake() {
        collider = GetComponent<Collider>();
        // collider.enabled = false;
    }

    public void ToggleFogWallActivation(bool _val) => collider.enabled = _val;
}
}
