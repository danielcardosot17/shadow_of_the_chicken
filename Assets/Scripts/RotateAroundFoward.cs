using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundFoward : MonoBehaviour
{
    [SerializeField] private float AngularSpeed;
    [SerializeField] private float rotationRange;

    private void FixedUpdate() {
        var angle = (transform.eulerAngles.z > 180) ? (transform.eulerAngles.z - 360) : transform.eulerAngles.z;
        if(angle >= rotationRange || angle <= (rotationRange*(-1))) AngularSpeed*=-1;
        transform.Rotate(Vector3.forward * AngularSpeed * Time.deltaTime);
    }
}
