using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    [SerializeField] private Transform RotationCenter;
    [SerializeField] private float AngularSpeed, RotationRadius, offsetY;
    [SerializeField] private float angle = 0;
    private float posX, posZ = 0;

    private void Start() {
        posX = RotationCenter.position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * RotationRadius;
        posZ = RotationCenter.position.z + Mathf.Sin(Mathf.Deg2Rad * angle) * RotationRadius;
        transform.position = new Vector3(posX,RotationCenter.position.y + offsetY,posZ);
    }
    private void FixedUpdate() {
        angle += AngularSpeed * Time.deltaTime;
        if(angle > 360) angle=0;

        posX = RotationCenter.position.x + Mathf.Sin(Mathf.Deg2Rad * angle) * RotationRadius;
        posZ = RotationCenter.position.z + Mathf.Cos(Mathf.Deg2Rad * angle) * RotationRadius;
        var newPosition = new Vector3(posX,RotationCenter.position.y + offsetY,posZ);
        var vectorDiff = newPosition - transform.position;
        
        transform.position = newPosition;
        transform.Rotate(Vector3.up * AngularSpeed * Time.deltaTime);
    }
}
