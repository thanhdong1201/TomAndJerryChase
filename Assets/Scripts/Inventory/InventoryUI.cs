using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryItemPrefab;

    private Dictionary<PowerUpBase, InventoryItem> powerUpDictionary;
    private List<InventoryItem> inventoryItems;

    private void Start()
    {
        powerUpDictionary = new Dictionary<PowerUpBase, InventoryItem>();
        inventoryItems = new List<InventoryItem>();
    }
    public void AddItem(PowerUpBase powerUp)
    {
        if (!powerUpDictionary.ContainsKey(powerUp))
        {
            GameObject inventoryItemGameObject = Instantiate(inventoryItemPrefab, transform);
            InventoryItem inventoryItem = inventoryItemGameObject.GetComponent<InventoryItem>();
            inventoryItem.AddItem(powerUp);
            powerUpDictionary.Add(powerUp, inventoryItem);
            inventoryItems.Add(inventoryItem);
        }
        if (powerUpDictionary.ContainsKey(powerUp))
        {
            powerUpDictionary.TryGetValue(powerUp, out InventoryItem inventoryItem);
            inventoryItem.AddItem(powerUp);
        }
    }
    public void Restart()
    {
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            inventoryItem.ResetData();
        }
    }
}
