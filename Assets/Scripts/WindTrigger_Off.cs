using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTrigger_Off : MonoBehaviour
{
    [SerializeField] private GameObject gameWind;
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(gameWind.activeInHierarchy){
                FindObjectOfType<AudioManager>().Stop("backgroundSFX");
                gameWind.SetActive(false);
            }
        }
    }
}
