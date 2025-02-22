using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioCueSO audioCueSO;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private float fadeOutDuration = 3.5f;
    private Coroutine fadeOutCoroutine;
    private float defaultMusicVolume;

    private void Awake()
    {
        defaultMusicVolume = musicSource.volume; // Lưu âm lượng gốc
    }

    private void OnEnable()
    {
        audioCueSO.OnPlaySFX += HandlePlaySFX;
        audioCueSO.OnPlayMusic += HandlePlayMusic;
        audioCueSO.OnFadeOutMusic += HandleFadeOutMusic;
    }

    private void OnDisable()
    {
        audioCueSO.OnPlaySFX -= HandlePlaySFX;
        audioCueSO.OnPlayMusic -= HandlePlayMusic;
        audioCueSO.OnFadeOutMusic -= HandleFadeOutMusic;
    }

    private void HandlePlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    private void HandlePlayMusic(AudioClip clip)
    {
        // If its fading out, stop the coroutine
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }

        musicSource.volume = defaultMusicVolume;
        musicSource.clip = clip;
        musicSource.Play();
    }

    private void HandleFadeOutMusic()
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(FadeOutCoroutine(fadeOutDuration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
        fadeOutCoroutine = null;
    }
}
