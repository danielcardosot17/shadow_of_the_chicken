using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private bool isRotating = false;
    private float rotationSpeed = 0.0f;

    private void Start() {
        isRotating = false;
        rotationSpeed = 0.0f;
    }
    private void Update() {
        if(isRotating){
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }    
    }

    public void RotateCamera(float speed){
        rotationSpeed = speed * 360;
        isRotating = true;
    }
    public void StopRotatingCamera(){
        rotationSpeed = 0.0f;
        isRotating = false;
    }
    public void ZoomIn(CinemachineVirtualCamera camera, Vector3 targetOffset, float zoomDuration){
        StartCoroutine(LerpPosition(camera, targetOffset, zoomDuration));
    }

    public void RotateThisAmount(Vector3 rotationAmount, float rotationDuration){
        StartCoroutine(LerpRotation(Quaternion.Euler(rotationAmount), rotationDuration));
    }

    IEnumerator LerpPosition(CinemachineVirtualCamera camera, Vector3 targetOffset, float zoomDuration)
    {
        float time = 0;
        Vector3 startPosition = camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        while (time < zoomDuration)
        {
            camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(startPosition, targetOffset, time / zoomDuration);
            time += Time.deltaTime;
            yield return null;
        }
        camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = targetOffset;
    }

    IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endValue;
    }
}
