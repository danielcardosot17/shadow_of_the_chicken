using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending1Trigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject godBox;
    [SerializeField] private float descendDuration;
    [SerializeField] private Vector3 descendPosition;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float orbitHeight;
    [SerializeField] private float orbitRadius;

    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")){
            if(Input.GetMouseButtonDown(0)){
                DescendToGodFight();
            }
        }
    }

    private void DescendToGodFight()
    {
        Destroy(godBox);
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        FindObjectOfType<AudioManager>().PlayDelayed("rumbleSFX");
        FindObjectOfType<CinemachineShake>().StartShake(amplitude,frequency);
        FindObjectOfType<CinemachineShake>().changeOrbit(0, orbitHeight, orbitRadius);
        StartCoroutine(LerpPosition(descendPosition, descendDuration));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = player.transform.position;

        while (time < duration)
        {
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        player.transform.position = targetPosition;
        FindObjectOfType<CinemachineShake>().StopShake();
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
    }
    
}
