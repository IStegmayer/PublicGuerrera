using UnityEngine;

namespace Dzajna {
public class ItemAction : ScriptableObject {
    public virtual void PerformAction(CharacterManager character) {
        Debug.Log("Performed Action");
    }
}
}