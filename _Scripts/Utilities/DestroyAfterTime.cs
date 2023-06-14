using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {
public class DestroyAfterTime : MonoBehaviour {
    public float timeUntilDestroyed = 1.5f;

    void Awake() {
        Destroy(gameObject, timeUntilDestroyed);
    }
}
}