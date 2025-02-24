using UnityEngine;
using System.Collections.Generic;

public class PlayerPowerUpHandler : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private List<PowerUpBase> powerUpInventory = new List<PowerUpBase>();
    [SerializeField] private PowerUpUI powerUpUI;

    private void OnEnable()
    {
        gameStateSO.OnGameStateChanged += HandleGameStateChange;
    }
    private void OnDisable()
    {
        gameStateSO.OnGameStateChanged -= HandleGameStateChange;
    }
    private void HandleGameStateChange(GameState newState)
    {
        if (newState == GameState.Restart) powerUpUI.Restart();
    }
    public void AddPowerUp(ItemType type)
    {
        foreach (PowerUpBase powerUpBase in powerUpInventory)
        {
            if (powerUpBase.type == type)
            {
                powerUpUI.AddItem(powerUpBase);
                return;
            }
        }
    }
}
