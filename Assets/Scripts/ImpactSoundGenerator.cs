using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSoundGenerator : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private bool justLanded = false;
    private float justLandedTimer = 0.0f;
    private CharacterController bossController;
    private AudioSource audioSource;

    void Start()
    {
        bossController = boss.GetComponent<CharacterController>();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(bossController.isGrounded){
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
            audioSource.Play();
        }
    }
}
