using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundTarget : MonoBehaviour
{
    [SerializeField] private Transform RotationCenter;
    [SerializeField] private float AngularSpeed, RotationRadius, offsetY;
    [SerializeField] private float angle;
    private float posX, posZ = 0;

    private void Start() {
        posX = RotationCenter.position.x + Mathf.Sin(Mathf.Deg2Rad * angle) * RotationRadius;
        posZ = RotationCenter.position.z + Mathf.Cos(Mathf.Deg2Rad * angle) * RotationRadius;
        transform.position = new Vector3(posX,RotationCenter.position.y + offsetY,posZ);
        transform.Rotate(Vector3.up * (angle - 90), Space.World);
    }
    private void FixedUpdate() {
        angle += AngularSpeed * Time.deltaTime;
        if(angle > 360) angle=0;

        posX = RotationCenter.position.x + Mathf.Sin(Mathf.Deg2Rad * angle) * RotationRadius;
        posZ = RotationCenter.position.z + Mathf.Cos(Mathf.Deg2Rad * angle) * RotationRadius;
        var newPosition = new Vector3(posX,transform.position.y,posZ);
        var vectorDiff = newPosition - transform.position;
        
        transform.position = newPosition;
        transform.Rotate(Vector3.up * AngularSpeed * Time.deltaTime, Space.World);
    }
}
