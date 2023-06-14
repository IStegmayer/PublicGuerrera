using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamageCollider : MonoBehaviour {
    [SerializeField] private Transform attackerStandPoint;
    
    public Transform GetAttackerStandPoint() => attackerStandPoint;

}
