using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerJumping : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bossTarget;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpPrepTime;
    [SerializeField] private float targetSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] [ColorUsageAttribute(true,true)] private Color white;
    [SerializeField] [ColorUsageAttribute(true,true)] private Color red;
    private CharacterController characterController;
    private Vector3 directionXZPlayerBoss;
    private Vector3 directionXZPlayerTarget;
    private Vector3 directionXZBossTarget;
    private float distanceXZPlayerBoss;
    private float distanceXZBossTarget;
    private float speedXZ = 0.0f;
    private float verticalSpeed = 0.0f;
    private bool canJump = true;
    private bool justLanded = false;
    private float justLandedTimer = 0.0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        directionXZPlayerBoss = Vector3.ProjectOnPlane(player.transform.position - this.transform.position,Vector3.up);
        directionXZPlayerTarget = Vector3.ProjectOnPlane(player.transform.position - bossTarget.transform.position,Vector3.up);
        distanceXZPlayerBoss = directionXZPlayerBoss.magnitude;
        directionXZBossTarget = Vector3.ProjectOnPlane(bossTarget.transform.position - this.transform.position,Vector3.up);
        distanceXZBossTarget = directionXZBossTarget.magnitude;
        ResetTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(characterController.isGrounded){
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
            ResetTarget();
        }

        if(characterController.isGrounded){
            directionXZPlayerTarget = Vector3.ProjectOnPlane(player.transform.position - bossTarget.transform.position,Vector3.up);
            directionXZPlayerBoss = Vector3.ProjectOnPlane(player.transform.position - this.transform.position,Vector3.up);
            distanceXZPlayerBoss = directionXZPlayerBoss.magnitude;
            speedXZ = 0;
            verticalSpeed=0;
            if(distanceXZPlayerBoss <= maxDistance && distanceXZPlayerBoss >= minDistance && player.transform.root != transform.root){
                LookAtPlayer();
                MoveTargetTowardsPlayer();
            }
            else{
                ResetTarget();
            }
            if(canJump){
                canJump = false;
                verticalSpeed = gravity * jumpTime/2;
                directionXZBossTarget = Vector3.ProjectOnPlane(bossTarget.transform.position - this.transform.position,Vector3.up);
                distanceXZBossTarget = directionXZBossTarget.magnitude;
                if(distanceXZBossTarget <= maxDistance && distanceXZBossTarget >= minDistance && player.transform.root != transform.root){
                    ChangeTargetColor(red);
                    bossTarget.transform.parent = null;
                    FindObjectOfType<AudioManager>().PlayDelayed("chickenCock");
                    speedXZ = distanceXZBossTarget/jumpTime;
                }
                StartCoroutine(DoAfterTimeCoroutine(jumpPrepTime + jumpTime, () => {canJump = true;}));
            }
        }

        verticalSpeed-=gravity * Time.deltaTime;
        var planeMove = directionXZBossTarget.normalized * speedXZ * Time.deltaTime;
        var verticalMove = Vector3.up * verticalSpeed * Time.deltaTime;
        characterController.Move(planeMove + verticalMove);
    }

    private void ChangeTargetColor(Color color)
    {
        bossTarget.GetComponent<Projector>().material.color = color;
    }

    private void ResetTarget()
    {
        bossTarget.transform.parent = transform.root;
        ChangeTargetColor(white);
        bossTarget.transform.position = new Vector3(transform.position.x,30,transform.position.z);
        bossTarget.GetComponent<Projector>().enabled = false;
    }

    private void MoveTargetTowardsPlayer()
    {
        bossTarget.GetComponent<Projector>().enabled = true;
        ChangeTargetColor(white);
        bossTarget.transform.position += directionXZPlayerTarget.normalized * targetSpeed * Time.deltaTime;
    }

    private void LookAtPlayer()
    {
        this.transform.forward = Vector3.Slerp(this.transform.forward, directionXZPlayerBoss.normalized, rotationSpeed);
    }
    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
