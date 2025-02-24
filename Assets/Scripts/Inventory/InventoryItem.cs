using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image itemImage;
    private float duration = 0f;
    public void AddItem(PowerUpBase powerUp)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        itemImage.sprite = powerUp.sprite;
        duration = powerUp.duration;
        powerUp.Activate();
        StartCoroutine(FillAmountCoroutine());
    }
    private void RemoveItem()
    {
        gameObject.SetActive(false);
    }
    private IEnumerator FillAmountCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fillValue = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            backgroundImage.fillAmount = fillValue;
            yield return null;
        }

        backgroundImage.fillAmount = 0f;
        RemoveItem();
    }
    public void ResetData()
    {
        itemImage.sprite = null;
        backgroundImage.fillAmount = 1f;
        gameObject.SetActive (false);
    }
}
