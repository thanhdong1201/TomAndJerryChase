using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/GameState", fileName = "GameState")]
public class GameStateSO : ScriptableObject
{
    public PlayerDataSO playerDataSO;
    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get => currentGameState;
        private set
        {
            if (currentGameState != value)
            {
                currentGameState = value;
                OnGameStateChanged?.Invoke(currentGameState);
            }
        }
    }

    public PlayerState CurrentPlayerState { get; private set; }
    public EnemyState CurrentEnemyState { get; private set; }

    public event Action<GameState> OnGameStateChanged;
    public event Action<PlayerState> OnPlayerStateChanged;
    public event Action<EnemyState> OnEnemyStateChanged;

    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;
    }
    public void SetPlayerState(PlayerState newState)
    {
        if (CurrentPlayerState == newState) return;
        CurrentPlayerState = newState;
        OnPlayerStateChanged?.Invoke(newState);

        if (CurrentPlayerState == PlayerState.Dead) SetGameState(GameState.GameOver);
    }

    public void SetEnemyState(EnemyState newState)
    {
        if (CurrentEnemyState == newState) return;
        CurrentEnemyState = newState;
        OnEnemyStateChanged?.Invoke(newState);
    }
    public void Restart()
    {
        CurrentGameState = GameState.Restart;
        playerDataSO.Restart();
    }
    private void OnEnable()
    {
        CurrentGameState = GameState.None;
    }

    public event Action OnObstacleCollision;

    public void TriggerObstacleCollision()
    {
        OnObstacleCollision?.Invoke();
    }
}
