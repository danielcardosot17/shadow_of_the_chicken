using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerPeck : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bossTarget;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;
    [SerializeField] private float minDistanceRetreat;
    [SerializeField] private float targetMinSize;
    [SerializeField] private float targetMaxSize;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpPrepTime;
    [SerializeField] private float peckPrepTime;
    [SerializeField] private float planeSpeed;
    // [SerializeField] private float peckMaxRange;
    // [SerializeField] private float peckMinRange;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float peckTime;
    [SerializeField] private float peckRecoilTime;
    [SerializeField] private float peckWaitTime;
    [SerializeField] private float peckAntecipationTime;
    [SerializeField] private float peckRotationX;
    [SerializeField] private float peckHeightY;
    [SerializeField] private GameObject impactOrigin;
    [SerializeField] private GameObject smokeExplosion;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] [ColorUsageAttribute(true,true)] private Color white;
    [SerializeField] [ColorUsageAttribute(true,true)] private Color red;
    private bool isPecking = false;
    private Quaternion originalRotation;
    private Quaternion targetPeckRotation;
    private CharacterController characterController;
    private Vector3 direction;
    private Vector3 directionXZ;
    private float distance;
    private float distanceXZ;
    private float speedXZ = 0.0f;
    private float verticalSpeed = 0.0f;
    private float jumpBufferTimer = 0.0f;
    private float peckAnimationTotalTime;
    private bool canPeck = true;
    private bool canJump = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        jumpBufferTimer = jumpPrepTime;
        direction = player.transform.position - this.transform.position;
        distance = direction.magnitude;
        directionXZ = Vector3.ProjectOnPlane(direction,Vector3.up);
        distanceXZ = directionXZ.magnitude;
        peckAnimationTotalTime = peckAntecipationTime + peckTime + peckWaitTime + peckRecoilTime;
        ResetTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction = player.transform.position - this.transform.position;
        distance = direction.magnitude;
        directionXZ = Vector3.ProjectOnPlane(direction,Vector3.up);
        distanceXZ = directionXZ.magnitude;
        if(characterController.isGrounded){
            speedXZ = 0;
            verticalSpeed=0;
            if(!isPecking){
                if(distanceXZ <= maxDistance && distanceXZ >= minDistance){
                    if(player.transform.root != transform.root){
                        LookAtPlayer();
                    }
                }
                if(canJump){
                    canJump = false;
                    StartCoroutine(DoAfterTimeCoroutine(jumpPrepTime, () => {canJump = true;}));
                    verticalSpeed = gravity * jumpTime/2;
                    if(distanceXZ <= maxDistance && distanceXZ >= minDistance){
                        speedXZ = planeSpeed;
                    }
                    else if(distanceXZ <= minDistance && distanceXZ >= minDistanceRetreat){
                        speedXZ = -2 * planeSpeed;
                    }
                }
            }
        }
        if(!isPecking){
            verticalSpeed-=gravity * Time.deltaTime;
            var planeMove = directionXZ.normalized * speedXZ * Time.deltaTime;
            var verticalMove = Vector3.up * verticalSpeed * Time.deltaTime;
            characterController.Move(planeMove + verticalMove);
        }
    }

    private void PeckPlayer()
    {
        // turn off trigger
        transform.GetComponent<BoxCollider>().enabled = false;
        // unparent Target
        bossTarget.transform.parent = null;

        isPecking = true;
        originalRotation = transform.rotation;
        targetPeckRotation = Quaternion.Euler(originalRotation.eulerAngles + Vector3.right * peckRotationX);
        // grow target and turn red durring antecipation
        StartCoroutine(LerpFunctionTargetSize(targetMaxSize, peckAntecipationTime, red));
        // start going up and rotating
        StartCoroutine(LerpPosition(transform.position + Vector3.up * peckHeightY,peckAntecipationTime + peckTime));
        // peckantecipation and PeckGo
        StartCoroutine(DoAfterTimeCoroutine(peckAntecipationTime,() => {StartCoroutine(PeckAnimationGo());}));
        // chicken cock
        FindObjectOfType<AudioManager>().PlayDelayed("chickenCock");
        // hit
        StartCoroutine(DoAfterTimeCoroutine(peckAntecipationTime + peckTime + peckWaitTime,() => {StartCoroutine(PeckAnimationBack());}));
        
    }

    private void LookAtPlayer()
    {
        this.transform.forward = Vector3.Slerp(this.transform.forward, directionXZ.normalized, rotationSpeed);
    }

    IEnumerator PeckAnimationGo()
    {
        yield return LerpFunction(targetPeckRotation, peckTime);
        Instantiate(smokeExplosion,impactOrigin.transform.position, impactOrigin.transform.rotation);
        audioSource.Play();
        StartCoroutine(DoAfterTimeCoroutine(peckWaitTime,() => {StartCoroutine(LerpPosition(transform.position - Vector3.up * peckHeightY,peckRecoilTime));}));
    }
    IEnumerator PeckAnimationBack()
    {
        yield return LerpFunction(originalRotation, peckRecoilTime);
        ResetTarget();
        // turn on trigger
        transform.GetComponent<BoxCollider>().enabled = true;
        isPecking = false;
    }
    IEnumerator LerpFunction(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endValue;
    }
    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
    
    private void ChangeTargetColor(Color color)
    {
        bossTarget.GetComponent<Projector>().material.color = color;
    }
    private void ResetTarget()
    {
        ChangeTargetColor(white);
        bossTarget.transform.parent = transform;
        StartCoroutine(LerpFunctionTargetSize(targetMinSize, peckAntecipationTime, white));
    }
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")){
            if(characterController.isGrounded){
                if(player.transform.root != transform.root){
                    if(canPeck){
                        canPeck = false;
                        StartCoroutine(DoAfterTimeCoroutine(peckPrepTime + peckAnimationTotalTime, () => {canPeck = true;}));
                        PeckPlayer();
                    }
                }
            }
        }
    }

    IEnumerator LerpFunctionTargetSize(float endValue, float duration, Color color)
    {
        float time = 0;
        float startValue = bossTarget.GetComponent<Projector>().orthographicSize;

        while (time < duration)
        {
            bossTarget.GetComponent<Projector>().orthographicSize = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        bossTarget.GetComponent<Projector>().orthographicSize = endValue;
        ChangeTargetColor(color);
    }
}
