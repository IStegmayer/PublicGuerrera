using System.Collections;
using UnityEngine;

namespace Dzajna
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem Weapon;

        public override void Interact(PlayerManager _playerManager) {
            base.Interact(_playerManager);
            PickUpItem(_playerManager);
        }

        private void PickUpItem(PlayerManager _playerManager) {
            CharacterInventoryManager characterInventoryManager;
            PlayerLocomotionManager _playerLocomotionManager;
            PlayerCharacterAnimatorManager playerCharacterAnimatorManager;
            InteractableUI _interactableUI;
            characterInventoryManager = _playerManager.GetComponent<CharacterInventoryManager>();
            _playerLocomotionManager = _playerManager.GetComponent<PlayerLocomotionManager>();
            playerCharacterAnimatorManager = _playerManager.GetComponent<PlayerCharacterAnimatorManager>();
            _interactableUI = FindObjectOfType<InteractableUI>();
            
            
            playerCharacterAnimatorManager.PlayTargetAnimation(Animator.StringToHash("PickUpItem"), true);
            characterInventoryManager.WeaponsInventory.Add(Weapon);
            _interactableUI.ItemPickupText.text = Weapon.ItemName;
            _interactableUI.ItemPickupImage.texture = Weapon.ItemIcon.texture;
            _interactableUI.ItemPickupUIGameObject.SetActive(true);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(DismissItemPickupRoutine(_interactableUI));
        }
        
        private IEnumerator DismissItemPickupRoutine(InteractableUI _interactableUI) {
            yield return new WaitForSeconds(2f);
            _interactableUI.ItemPickupUIGameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}