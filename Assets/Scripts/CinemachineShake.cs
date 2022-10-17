using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CinemachineShake : MonoBehaviour
{
    // [SerializeField] private GameObject player;
    
    // [Range(0.1f,5f)]
    // [SerializeField] private float amplitude;
    
    // [Range(0.1f,5f)]
    // [SerializeField] private float frequency;
    private CinemachineFreeLook playerCamera;
    private List<CinemachineVirtualCamera> cameraRigs;
    private List<CinemachineBasicMultiChannelPerlin> cameraNoises;
    // private bool isShaking = false;

    private void Awake() {
        playerCamera = GetComponent<CinemachineFreeLook>();
        cameraRigs = new List<CinemachineVirtualCamera>();
        cameraNoises = new List<CinemachineBasicMultiChannelPerlin>();
        cameraRigs.Add(playerCamera.GetRig(0));
        cameraRigs.Add(playerCamera.GetRig(1));
        cameraRigs.Add(playerCamera.GetRig(2));
        foreach(CinemachineVirtualCamera camera in cameraRigs){
            cameraNoises.Add(camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>());
        }
        StopShake();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if(player.transform.root.CompareTag("Boss")){
    //         if(!isShaking) StartShake(amplitude, frequency);
    //     }
    //     else{
    //         if(isShaking) StopShake();
    //     }
    // }

    public void StartShake(float amp, float freq)
    {
        foreach(CinemachineBasicMultiChannelPerlin noise in cameraNoises){
            noise.m_AmplitudeGain = amp;
            noise.m_FrequencyGain = freq;
        }
        // isShaking = true;
    }

    public void StopShake()
    {
        foreach(CinemachineBasicMultiChannelPerlin noise in cameraNoises){
            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
        }
        // isShaking = false;
    }

    public void changeOrbit(int orbit, float height, float radius){
        playerCamera.m_Orbits[orbit].m_Height = height;
        playerCamera.m_Orbits[orbit].m_Radius = radius;
    }
}
