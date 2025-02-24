using UnityEngine;

public class Collectible : MonoBehaviour
{
    public ItemType itemType;

    [Header("Broadcasting from Events")]
    [SerializeField] protected GameObjectEventChannelSO returnToPoolEvent;

    public void Collect()
    {
        gameObject.SetActive(false);
        returnToPoolEvent?.RaiseEvent(gameObject);
    }
}

public enum ItemType
{
    Coin,
    Invisibility,
    SpeedBoost,
    GainLife,
    Shield,
    Magnet
}
