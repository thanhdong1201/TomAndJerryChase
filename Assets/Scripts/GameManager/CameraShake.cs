using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [Header("Listen to Events")]
    [SerializeField] private VoidEventChannelSO cameraShakeHardEventChannelSO;
    [SerializeField] private VoidEventChannelSO cameraShakeSoftEventChannelSO;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera[] cameras;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeCoroutine;

    private void OnEnable()
    {
        cameraShakeHardEventChannelSO.OnEventRaised += HandleCameraShakeHard;
        cameraShakeSoftEventChannelSO.OnEventRaised += HandleCameraShakeSoft;
    }
    private void OnDisable()
    {
        cameraShakeHardEventChannelSO.OnEventRaised -= HandleCameraShakeHard;
        cameraShakeSoftEventChannelSO.OnEventRaised -= HandleCameraShakeSoft;
    }
    private void HandleCameraShakeHard()
    {
        ShakeCamera(1.5f, 1f);
    }
    private void HandleCameraShakeSoft()
    {
        ShakeCamera(1f, 0.5f);
    }
    private void ShakeCamera(float intensity, float time)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        FindHighestPriorityCamera();
        shakeCoroutine = StartCoroutine(ShakeRoutine(intensity, time));
    }
    private IEnumerator ShakeRoutine(float intensity, float time)
    {
        noise.m_AmplitudeGain = intensity;  // Cường độ rung
        noise.m_FrequencyGain = 10f;  // Tần số rung cố định (có thể điều chỉnh)

        yield return new WaitForSeconds(time);

        // Giảm dần rung về 0 để tránh giật
        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            noise.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        noise.m_AmplitudeGain = 0f;
        shakeCoroutine = null;
    }
    private void FindHighestPriorityCamera()
    {
        if (cameras.Length == 0) return;

        cinemachineVirtualCamera = cameras[0];
        int highestPriority = cinemachineVirtualCamera.Priority;

        foreach (var cam in cameras)
        {
            if (cam.Priority > highestPriority && cam.isActiveAndEnabled)
            {
                cinemachineVirtualCamera = cam;
                highestPriority = cam.Priority;
            }
        }

        noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
}
