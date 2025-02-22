using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Data/AudioCue", fileName = "AudioCue")]

public class AudioCueSO : ScriptableObject
{
    public event Action<AudioClip> OnPlaySFX;
    public event Action<AudioClip> OnPlayMusic;
    public event Action OnFadeOutMusic;

    public void PlaySFX(AudioClip newClip)
    {
        OnPlaySFX?.Invoke(newClip);
    }
    public void PlayMusic(AudioClip newClip)
    {
        OnPlayMusic?.Invoke(newClip);
    }
    public void FadeOutMusic()
    {
        OnFadeOutMusic?.Invoke();
    }
}
