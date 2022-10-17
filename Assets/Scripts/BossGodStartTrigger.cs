using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGodStartTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bossGod;
    [SerializeField] private GameObject vastOuterSpaceSkybox;
    [SerializeField] private float stopDelay;
    [SerializeField] private float audioDelay;
    [SerializeField] private float presentationDelay;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject[] objectsNotToDeactivate;
    [SerializeField] private GameObject directionalLight;
    [SerializeField] private Vector3 lightRotation;

    private void Awake() {
        vastOuterSpaceSkybox.SetActive(false); 
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            FindObjectOfType<AudioManager>().StopAllExcept(new string[]{"backgroundSFX"}, stopDelay);
            FindObjectOfType<AudioManager>().PlayDelayed("bossGodMusic",audioDelay);
            FindObjectOfType<CameraController>().PresentBoss("godPresent", canvas, presentationDelay);
            FindObjectOfType<CameraController>().DeactivateAllObjectsExcept(objectsNotToDeactivate);
            StartCoroutine(DoAfterTimeCoroutine(presentationDelay*2,() => {
                bossGod.GetComponent<Animator>().enabled = true;
            }));
            FindObjectOfType<PlayerMovement>().ResetTurboTimer();
            RenderSettings.fog = false;
            RenderSettings.skybox = null;
            vastOuterSpaceSkybox.SetActive(true);
            directionalLight.transform.rotation = Quaternion.Euler(lightRotation);
            directionalLight.GetComponent<RotateAroundUp>().enabled = true;
            StartCoroutine(DoAfterTimeCoroutine(presentationDelay*3,() => {
                this.gameObject.SetActive(false);
            }));
        }
    }
    
    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
