using System;
using UnityEngine;

namespace Dzajna
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private float radius = 0.6f;
        public string InteractableText;
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager) {
            Debug.Log("Interacted with object");
        }
    }
}