using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOutOfBoundsTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 teleportPosition;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float flashBackAudioTime;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            FindObjectOfType<AudioManager>().PlayDelayed("flashbackSFX");
            FindObjectOfType<AudioManager>().PlayDelayed("rumbleSFX");
            FindObjectOfType<CinemachineShake>().StartShake(amplitude,frequency);
            other.GetComponent<PlayerMovement>().enabled = false;
            
            StartCoroutine(DoAfterTimeCoroutine(flashBackAudioTime,() => {
                FindObjectOfType<CinemachineShake>().StopShake();
                FindObjectOfType<AudioManager>().StopAllExcept(new string[]{"bossWingsMusic", "bossGodMusic", "bossLegsMusic", "bossNeckMusic","backgroundSFX"});
                other.GetComponent<PlayerMovement>().enabled = true;
                if(other.transform.root.CompareTag("Boss")){
                    other.transform.position = other.transform.root.position + Vector3.up * 350;
                }
                else{
                    other.transform.position = teleportPosition;
                }
            }));
        }
        else{
            other.GetComponent<ReturnToOriginalPosition>().ReturnToOrigin();
        }
    }
    
    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
