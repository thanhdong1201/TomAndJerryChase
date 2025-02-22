using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private int count = 0;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countText;
    public PowerUpBase powerUpBase { get; private set; }

    public void IncreaseCount()
    {
        count++;
        countText.text = count.ToString();  
    }
    public void AddItem(PowerUpBase powerUp)
    {
        powerUpBase = powerUp;
        image.sprite = powerUp.powerUpIcon;
    }
    public void RemoveItem()
    {
        count--;
        countText.text = count.ToString();
        if (count == 0)
        {
            count = 0;
            Destroy(gameObject);
        }
    }
    public void UseItem()
    {
        powerUpBase.Activate();
        RemoveItem();
    }
}
