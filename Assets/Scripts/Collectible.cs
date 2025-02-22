using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Type type;
    public PowerUpType powerUpType;

    [Header("Broadcasting from Events")]
    [SerializeField] private GameObjectEventChannelSO coinEvent;
    [SerializeField] private GameObjectEventChannelSO powerUpEvent;

    public void Collect()
    {
        gameObject.SetActive(false);
        if (type == Type.Cheese)
        {
            
            coinEvent?.RaiseEvent(gameObject);
        }
        else
        {
            powerUpEvent?.RaiseEvent(gameObject);
        }
    }
}

public enum Type
{
    Cheese,
    PowerUp
}
