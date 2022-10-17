using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending0Trigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")){
            if(Input.GetMouseButtonDown(0)){
                FindObjectOfType<CameraController>().StartEnding0();
                Destroy(this.gameObject);
            }
        }
    }
}
