using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNeckStartTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bossNeck;
    [SerializeField] private float stopDelay;
    [SerializeField] private float audioDelay;
    [SerializeField] private float presentationDelay;
    [SerializeField] private Canvas canvas;
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            FindObjectOfType<AudioManager>().StopAllExcept(new string[]{"backgroundSFX"}, stopDelay);
            FindObjectOfType<AudioManager>().PlayDelayed("bossNeckMusic",audioDelay);
            bossNeck.GetComponent<Animator>().enabled = true;
            FindObjectOfType<CameraController>().PresentBoss("neckPresent", canvas, presentationDelay);
            FindObjectOfType<PlayerMovement>().ResetTurboTimer();
            Destroy(this.gameObject);
        }
    }
}
