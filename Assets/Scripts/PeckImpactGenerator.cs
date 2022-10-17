using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeckImpactGenerator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float impactMaxRadius;
    [SerializeField] private float impactMinRadius;
    [SerializeField] private float addedY;
    [SerializeField] private float addedXZ;
    [SerializeField] private float impactDuration;
    [SerializeField] private float impactPrepTime;
    private bool canImpact = true;
    private Vector3 direction;
    private float distance;
    private Vector3 directionXZ;
    private float distanceXZ;
    private Vector3 impact = Vector3.zero;
    private CharacterController playerController;

    void Start()
    {
        playerController = player.GetComponent<CharacterController>();
        direction = playerController.transform.position - transform.position;
        distance = direction.magnitude;
        directionXZ = direction.x * Vector3.right + direction.z * Vector3.forward;
        distanceXZ = directionXZ.magnitude;
    }

    private void OnTriggerEnter(Collider other) {
        if(canImpact){
            canImpact = false;
            StartCoroutine(DoAfterTimeCoroutine(impactPrepTime, () => {canImpact = true;}));
            if(other.CompareTag("Player")){
                direction = playerController.transform.position - transform.position;
                distance = direction.magnitude;
                directionXZ = direction.x * Vector3.right + direction.z * Vector3.forward;
                distanceXZ = directionXZ.magnitude;
                if(distanceXZ <= impactMaxRadius && playerController.isGrounded && player.transform.root != transform.root){
                    var verticalMove = Vector3.up * addedY;
                    var planeMove = Vector3.ProjectOnPlane(direction,Vector3.up).normalized * addedXZ;
                    var distanceMultiplier = 0.0f;
                    if(distanceXZ <= impactMinRadius){
                        distanceMultiplier = 1;
                    }
                    else{
                        distanceMultiplier = Mathf.Lerp(1,0,(distanceXZ-impactMinRadius)/(impactMaxRadius-impactMinRadius));
                    }
                    impact = (planeMove + verticalMove) * distanceMultiplier;
                    StartCoroutine(LerpFunction(0, impactDuration));
                }
            }
        }
    }

    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        Vector3 startVector = impact;

        while (time < duration)
        {
            impact = Vector3.Lerp(startVector, Vector3.zero, time / duration);
            playerController.Move(impact * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        playerController.Move(Vector3.zero * Time.deltaTime);
    }
    
    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
