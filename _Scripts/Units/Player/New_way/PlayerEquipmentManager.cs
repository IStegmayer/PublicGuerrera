using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: refactor this without using a collider
namespace Dzajna {
public class PlayerEquipmentManager : MonoBehaviour {
    private CharacterInventoryManager characterInventoryManager;

    private void Awake() {
        characterInventoryManager = GetComponent<CharacterInventoryManager>();
    }
}
}