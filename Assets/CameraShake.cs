using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer = 0f;
    private bool doShake = false;
    public static CameraShake Instance { get;  private set; }

    void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera= GetComponent<CinemachineVirtualCamera>();
    }


    public void Shake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain= intensity;
        shakeTimer = time;
        doShake = true;
    }

    private void Update()
    {
        if(doShake && shakeTimer > 0)
        {
            shakeTimer-= Time.deltaTime;

        }
        else if(doShake)
        {
            shakeTimer= 0f;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            doShake = false;
        }
    }


}
