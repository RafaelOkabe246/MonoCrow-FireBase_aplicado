using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField]
    private CinemachineVirtualCamera cmVCam;

    //Camera Shake
    private IEnumerator cameraShakeCoroutine;
    private bool isCameraShaking;
    private float curAmplitude, curFrequency;

    private void Awake()
    {
        instance = this;
    }

    public void CameraShake(float amplitude, float frequency, float time)
    {
        //Using this verification, we are able to override the current camera shakes with stronger shakes
        if (isCameraShaking && (curAmplitude < amplitude && curFrequency < frequency))
            StopCameraShake();

        if (isCameraShaking)
            return;

        cameraShakeCoroutine = ShakeCamera(amplitude, frequency, time);
        StartCoroutine(cameraShakeCoroutine);
    }

    public IEnumerator ShakeCamera(float amplitude, float frequency, float time)
    {
        cmVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cmVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;

        isCameraShaking = true;

        yield return new WaitForSecondsRealtime(time);

        StopCameraShake();
    }

    public void StopCameraShake()
    {
        StopCoroutine(cameraShakeCoroutine);
        cmVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        cmVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
        isCameraShaking = false;
    }
}
