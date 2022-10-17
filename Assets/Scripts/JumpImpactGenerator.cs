using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpImpactGenerator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;
    [SerializeField] private float impactMaxRadius;
    [SerializeField] private float impactMinRadius;
    [SerializeField] private float addedY;
    [SerializeField] private float addedXZ;
    [SerializeField] private float impactDuration;
    [SerializeField] private GameObject smokeExplosion;
    private Vector3 direction;
    private float distance;
    private Vector3 directionXZ;
    private float distanceXZ;
    private bool justLanded = false;
    private float justLandedTimer = 0.0f;
    private Vector3 impact = Vector3.zero;
    private CharacterController playerController;
    private CharacterController bossLegsController;
    private AudioSource audioSource;

    void Start()
    {
        playerController = player.GetComponent<CharacterController>();
        bossLegsController = boss.GetComponent<CharacterController>();
        audioSource = this.GetComponent<AudioSource>();
        direction = playerController.transform.position - transform.position;
        distance = direction.magnitude;
        directionXZ = direction.x * Vector3.right + direction.z * Vector3.forward;
        distanceXZ = directionXZ.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction = playerController.transform.position - transform.position;
        distance = direction.magnitude;
        directionXZ = direction.x * Vector3.right + direction.z * Vector3.forward;
        distanceXZ = directionXZ.magnitude;

        if(bossLegsController.isGrounded){
            if(justLandedTimer <= 0){
                justLanded = true;
            }
            else{
                justLanded = false;
            }
            justLandedTimer += Time.deltaTime;
        }else{
            justLandedTimer = 0;
            justLanded = false;
        }

        if(justLanded){
            Instantiate(smokeExplosion,transform.position, Quaternion.identity);
            audioSource.Play();
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
}
