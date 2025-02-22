using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeController : MonoBehaviour
{
    [Header("Listen to Events")]
    [SerializeField] private BoolEventChannelSO applyMotionBlurEvent;
    [SerializeField] private VoidEventChannelSO applyVintageEvent;

    private Volume volume;
    private MotionBlur motionBlur;
    private Bloom bloom;
    private Vignette vignette;

    private void Start()
    {
        volume = GetComponent<Volume>();

        volume.profile.TryGet(out motionBlur);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out vignette);
    }
    private void OnEnable()
    {
        applyMotionBlurEvent.OnEventRaised += HandleApplyMotionBlur;
        applyVintageEvent.OnEventRaised += HandleApplyVintage;
    }
    private void OnDisable()
    {
        applyMotionBlurEvent.OnEventRaised -= HandleApplyMotionBlur;
        applyVintageEvent.OnEventRaised -= HandleApplyVintage;
    }
    private void HandleApplyMotionBlur(bool value)
    {
        motionBlur.active = value;

        if (value)
        {
            motionBlur.intensity.value = 0.5f;
        }
        else
        {
            motionBlur.intensity.value = 0f;
        }
    }
    private void HandleApplyVintage()
    {  
        StartCoroutine(ApplyVintage());
    }
    private IEnumerator ApplyVintage()
    {
      
        vignette.intensity.value = 0.5f;
        yield return new WaitForSeconds(1f);
        vignette.intensity.value = 0f;
        vignette.active = false;
    }
}
