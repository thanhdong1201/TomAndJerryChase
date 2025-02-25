using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private AudioCueSO audioCueSO;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameOverMusic;

    private float fadeOutDuration = 3.5f;
    private Coroutine fadeOutCoroutine;
    private float defaultMusicVolume;

    private void Awake()
    {
        defaultMusicVolume = musicSource.volume;
    }
    private void OnEnable()
    {
        gameStateSO.OnGameStateChanged += HandleGameStateChange;
        audioCueSO.OnPlaySFX += HandlePlaySFX;
        audioCueSO.OnPlayMusic += HandlePlayMusic;
        audioCueSO.OnFadeOutMusic += HandleFadeOutMusic;
    }

    private void OnDisable()
    {
        gameStateSO.OnGameStateChanged -= HandleGameStateChange;
        audioCueSO.OnPlaySFX -= HandlePlaySFX;
        audioCueSO.OnPlayMusic -= HandlePlayMusic;
        audioCueSO.OnFadeOutMusic -= HandleFadeOutMusic;
    }
    private void Start()
    {
        HandlePlayMusic(menuMusic);
    }
    private void HandleGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Pause:
                sfxSource.Stop();
                musicSource.Pause();   
                break;
            case GameState.Resume:
                musicSource.UnPause();
                break;
            case GameState.Start:
                musicSource.Stop();
                break;
            case GameState.Restart:
                musicSource.Stop();
                HandlePlayMusic(menuMusic);
                break;
            case GameState.GameOver:
                HandleFadeOutMusic();
                HandlePlaySFX(gameOverMusic);
                break;
        }
    }
    private void HandlePlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    private void HandlePlayMusic(AudioClip clip)
    {
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
