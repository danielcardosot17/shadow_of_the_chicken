using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundRight : MonoBehaviour
{
    [SerializeField] private float AngularSpeed;
    [SerializeField] private float rotationRange;

    private void FixedUpdate() {
        var angle = (transform.eulerAngles.x > 180) ? (transform.eulerAngles.x - 360) : transform.eulerAngles.x;
        if(angle >= rotationRange || angle <= (rotationRange*(-1))) AngularSpeed*=-1;
        transform.Rotate(Vector3.right * AngularSpeed * Time.deltaTime);
    }
}
