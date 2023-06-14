using UnityEngine;

namespace Dzajna {
public abstract class State : MonoBehaviour {
    public abstract State Tick(EnemyManager enemy);
}
}