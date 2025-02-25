using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDebug : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private PlayerDataSO playerData;

    [Header("DebugUI")]
    [SerializeField] private TextMeshProUGUI currentGameStateText;
    [SerializeField] private TextMeshProUGUI currentPlayerStateText;
    [SerializeField] private TextMeshProUGUI currentEnemyStateText;
    [SerializeField] private TextMeshProUGUI fpsText;

    private float deltaTime = 0.0f;

    private void OnEnable()
    {
        gameStateSO.OnGameStateChanged += HandleGameStateUI;
        gameStateSO.OnPlayerStateChanged += HandlePlayerState;
        gameStateSO.OnEnemyStateChanged += HandleEnemyState;
    }
    private void OnDisable()
    {
        gameStateSO.OnGameStateChanged -= HandleGameStateUI;
        gameStateSO.OnPlayerStateChanged -= HandlePlayerState;
        gameStateSO.OnEnemyStateChanged -= HandleEnemyState;
    }
    private void Update()
    {
        ShowFPS();
    }
    private void ShowFPS()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.SetText("FPS: " + Mathf.Ceil(fps));
    }
    private void HandlePlayerState(PlayerState playerState)
    {
        currentPlayerStateText.SetText("Player: " + playerState.ToString());
    }
    private void HandleEnemyState(EnemyState enemyState)
    {
        currentEnemyStateText.SetText("Enemy: " + enemyState.ToString());
    }
    private void HandleGameStateUI(GameState state)
    {
        currentGameStateText.SetText("State: " + state.ToString());
    }
}
