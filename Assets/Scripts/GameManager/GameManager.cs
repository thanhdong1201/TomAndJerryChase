using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public enum GameState
{
    None,
    Start,
    Restart,
    GameOver,
    Pause,
    Resume
}

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameStateSO gameStateSO;

    [Header("Camera")]
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private GameObject startCamera;
    [SerializeField] private GameObject chaseCamera;

    [Header("Music & Sound")]
    [SerializeField] private AudioCueSO audioCueSO;
    [SerializeField] private AudioClip gameOverMusic;

    private Coroutine restartCoroutine;
    private GameObject player;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        gameStateSO.SetGameState(GameState.None);
        player = GameObject.FindWithTag("Player");

        gameStateSO.OnGameStateChanged += HandleGameStateChange;
        gameStateSO.OnEnemyStateChanged += HandleEnemyStateChange;
    }
    private void OnDisable()
    {
        gameStateSO.OnGameStateChanged -= HandleGameStateChange;
        gameStateSO.OnEnemyStateChanged -= HandleEnemyStateChange;
    }
    private void HandleGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Pause:
                PauseGame();
                break;
            case GameState.Resume:
                ResumeGame();
                break;
            case GameState.Start:
                StartGame();
                break;
            case GameState.Restart:
                RestartGame();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
    }
    private void HandleEnemyStateChange(EnemyState newState)
    {
        switch (newState)
        {
            case EnemyState.Lost:
                StopChase();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
        }
    }
    private void Chase()
    {
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
        chaseCamera.SetActive(true);
    }
    private void StopChase()
    {
        chaseCamera.SetActive(false);
    }
    private void GameOver()
    {
        audioCueSO.PlaySFX(gameOverMusic);
    }
    private void PauseGame()
    {
        Time.timeScale = 0f;
    }
    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    private void StartGame()
    {
        Time.timeScale = 1f;
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
        startCamera.SetActive(false);
    }
    private void RestartGame()
    {
        if (restartCoroutine != null) StopCoroutine(restartCoroutine);
        restartCoroutine = StartCoroutine(RestartCoroutine());
    }
    private IEnumerator RestartCoroutine()
    {
        // Giải phóng tài nguyên không dùng
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        // Đợi 1 frame để đảm bảo giải phóng hoàn tất
        yield return null;

        Time.timeScale = 1f;
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        chaseCamera.SetActive(false);
        startCamera.SetActive(true);
        audioCueSO.FadeOutMusic();
    }
}