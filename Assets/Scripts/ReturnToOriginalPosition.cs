using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToOriginalPosition : MonoBehaviour
{
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Vector3 originalRotation;
    public void ReturnToOrigin(){
        transform.position = originalPosition;
        transform.rotation = Quaternion.Euler(originalRotation);
        FindObjectOfType<AudioManager>().PlayDelayed("chickenCock");
    }
}
