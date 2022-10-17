using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWingsStartTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bossWings;
    [SerializeField] private float audioDelay;
    [SerializeField] private float presentationDelay;
    [SerializeField] private Canvas canvas;
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            FindObjectOfType<AudioManager>().PlayDelayed("bossWingsMusic",audioDelay);
            bossWings.GetComponent<Animator>().enabled = true;
            FindObjectOfType<CameraController>().PresentBoss("wingsPresent", canvas, presentationDelay);
            FindObjectOfType<PlayerMovement>().ResetTurboTimer();
            Destroy(this.gameObject);
        }
    }
}
