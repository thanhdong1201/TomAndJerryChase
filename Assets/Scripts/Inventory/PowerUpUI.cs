using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PowerUpUI : MonoBehaviour
{
    public List<InventorySlot> inventorySlots;
    public GameObject inventoryItemPrefab; 
    public Transform inventoryPanel;

    public void AddItem(PowerUpBase powerUp)
    {
        // Kiểm tra xem item đã tồn tại trong inventory chưa
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.powerUpBase == powerUp)
            {
                itemInSlot.IncreaseCount();
                return;
            }
        }

        // Nếu không có item nào cùng loại, tìm slot trống và tạo item mới
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount == 0) // Kiểm tra slot trống
            {
                InitializeItem(powerUp, slot.transform);
                return;
            }
        }
    }
    private void InitializeItem(PowerUpBase powerUp , Transform inventorySlotTransform)
    {
        GameObject go = Instantiate(inventoryItemPrefab, inventorySlotTransform);
        InventoryItem item = go.GetComponent<InventoryItem>();
        item.AddItem(powerUp);
        item.IncreaseCount();
    }
    public void Restart()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);
            }
        }
    }
}
