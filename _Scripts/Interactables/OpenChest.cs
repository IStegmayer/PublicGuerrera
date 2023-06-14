using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dzajna {
public class OpenChest : Interactable {
    [SerializeField] private Transform playerStandingPosition;
    [SerializeField] private GameObject itemSpawner;
    [SerializeField] private WeaponItem itemInChest;
    
    private Animator animator;
    private OpenChest openChest;

    private void Awake() {
        animator = GetComponent<Animator>();
        openChest = GetComponent<OpenChest>();
    }

    public override void Interact(PlayerManager playerManager) {
        base.Interact(playerManager);
        Vector3 rotationDirection = transform.position - playerManager.transform.position;
        rotationDirection.y = 0;
        rotationDirection.Normalize();
        
        Quaternion tr = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
        playerManager.transform.rotation = targetRotation;
        
        playerManager.OpenChestInteraction(playerStandingPosition);
        animator.Play("OpenChest");
        StartCoroutine(SpawnItemInChest());

        WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();
        weaponPickUp.Weapon = itemInChest;
    }
    private IEnumerator SpawnItemInChest() {
        yield return new WaitForSeconds(1f);
        Instantiate(itemSpawner, transform);
        Destroy(openChest);
    }
}
}
