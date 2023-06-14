using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Dzajna {
public class OpenDoor : Interactable {
    [SerializeField] private Transform playerStandingPosition;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private Transform leftDoor;
    [SerializeField] private float animDuration;
    [SerializeField] private AnimationCurve animationCurve;
    private Quaternion initialRotation;
    private OpenDoor openDoor;

    private void Start() {
        initialRotation = rightDoor.rotation;
        openDoor = GetComponent<OpenDoor>();
    }

    public override void Interact(PlayerManager playerManager) {
        base.Interact(playerManager);
        
        playerManager.transform.position = playerStandingPosition.position;
        playerManager.transform.rotation = playerStandingPosition.rotation;
        
        CharacterAnimatorManager anim = playerManager.GetComponent<CharacterAnimatorManager>();
        anim.PlayTargetAnimation("OpenDoubleDoor", true);
        // animate door
        StartCoroutine(OpenDoors());
    } 
    
    private IEnumerator OpenDoors()
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < animDuration) {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / animDuration);
            float curveValue = animationCurve.Evaluate(t);
            Quaternion targetRotation = initialRotation * Quaternion.Euler(0, -curveValue * 80.0f, 0);
            leftDoor.rotation = targetRotation;
            targetRotation = initialRotation * Quaternion.Euler(0, curveValue * 80.0f, 0);
            rightDoor.rotation = targetRotation;
            yield return null;
        }
        
        Destroy(openDoor);
    }
}
}