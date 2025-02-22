using System.Collections;
using UnityEngine;
public enum PowerUpType
{
    None,
    Invisibility,
    SpeedBoost,
    GainLife,
    Shield, 
    Magnet
}

public abstract class PowerUpBase : MonoBehaviour
{
    [SerializeField] protected GameStateSO gameStateSO;
    [SerializeField] protected GameObject effect;
    public abstract PowerUpType type { get; }
    public Sprite powerUpIcon;
    public float duration = 5f;
    protected GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public void Activate()
    {
        effect.SetActive(true);
        ApplyEffect();
        UpdatePlayerState(true);
        player.GetComponent<PlayerPowerUpHandler>().StartCoroutine(DeactivateAfterDuration());
    }

    protected abstract void ApplyEffect();
    protected abstract void RemoveEffect();

    private IEnumerator DeactivateAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        effect.SetActive(false);
        RemoveEffect();
        gameStateSO.SetPlayerState(PlayerState.Running);
    }
    private void UpdatePlayerState(bool isActive)
    {
        switch (type)
        {
            case PowerUpType.Invisibility:
                gameStateSO.SetPlayerState(PlayerState.Invisible);
                break;
            case PowerUpType.SpeedBoost:
                gameStateSO.SetPlayerState(PlayerState.SpeedBoost);
                break;
            case PowerUpType.Shield:
                gameStateSO.SetPlayerState(PlayerState.Shielded);
                break;
            case PowerUpType.Magnet:
                gameStateSO.SetPlayerState(PlayerState.Magnet);
                break;
        }
    }
}
