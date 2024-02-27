using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shake : MonoBehaviour
{
    public static Shake Instance { get; private set; }
    private CinemachineVirtualCamera virtCamera;
    private float shakeTimer;
    bool isShaking = false;

    private void Awake()
    {
        Instance = this;
        virtCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void CamShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin camPerlin = virtCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Dash") && !isShaking)
        {
            StartCoroutine(DashShaking());
        }

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin camPerlin = virtCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                camPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
    public IEnumerator DashShaking()
    {
        Shake.Instance.CamShake(1f, 0.4f);
        isShaking = true;
        yield return new WaitForSeconds(1f);
        Shake.Instance.CamShake(0, 0.4f);
        isShaking = false;
    }

    public IEnumerator DamagedShaking()
    {
        yield return new WaitForSeconds(0.25f);
        Shake.Instance.CamShake(2.5f, 3.5f);
        isShaking = true;
        yield return new WaitForSeconds(0.2f);
        Shake.Instance.CamShake(0, 2.5f);
        yield return new WaitForSeconds(2.5f);
        isShaking = false;
    }
}
