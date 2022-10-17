using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundUp : MonoBehaviour
{
    [SerializeField] private float AngularSpeed;
    [SerializeField] private float rotationRange;

    private void FixedUpdate() {
        var angle = (transform.eulerAngles.y > 180) ? (transform.eulerAngles.y - 360) : transform.eulerAngles.y;
        if(angle >= rotationRange || angle <= (rotationRange*(-1))) AngularSpeed*=-1;
        transform.Rotate(Vector3.up * AngularSpeed * Time.deltaTime);
    }
}
