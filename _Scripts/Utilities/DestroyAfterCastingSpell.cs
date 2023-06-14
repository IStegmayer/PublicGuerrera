using System;
using System.Collections;
using System.Collections.Generic;
using Dzajna;
using UnityEngine;

public class DestroyAfterCastingSpell : MonoBehaviour {
    private CharacterManager casterManager;

    private void Awake() {
        casterManager = GetComponentInParent<CharacterManager>();
    }

    private void Update() {
        if (casterManager.IsFiringSpell) Destroy(gameObject);
    }
}
