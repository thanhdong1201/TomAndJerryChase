using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private List<PowerUpBase> powerUpInventory = new List<PowerUpBase>();
    [SerializeField] private InventoryUI inventoryUI;

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
        if (newState == GameState.Restart) inventoryUI.Restart();
    }
    public void AddPowerUp(ItemType type)
    {
        foreach (PowerUpBase powerUpBase in powerUpInventory)
        {
            if (powerUpBase.type == type)
            {
                inventoryUI.AddItem(powerUpBase);
                return;
            }
        }
    }
    public void RemovePowerUp(ItemType type)
    {
        foreach (PowerUpBase powerUpBase in powerUpInventory)
        {
            if (powerUpBase.type == type)
            {
                powerUpBase.DisableEffect();
                return;
            }
        }
    }
}
