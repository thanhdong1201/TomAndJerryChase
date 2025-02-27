using System.Collections;
using UnityEngine;


public abstract class PowerUpBase : MonoBehaviour
{
    [SerializeField] protected GameStateSO gameStateSO;
    [SerializeField] protected ParticleSystem particle;

    public abstract ItemType type { get; }
    public Sprite sprite;
    public float duration = 5f;

    protected GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public void Activate()
    {
        ApplyEffect();
    }
    protected virtual void ApplyEffect()
    {
        UpdatePlayerState();
        particle.gameObject.SetActive(true);
        particle.Play();
        StartCoroutine(RemoveEffectAfterDuration());
    }
    protected virtual void RemoveEffect() 
    {
        particle.Stop();
        particle.gameObject.SetActive(false);
        gameStateSO.SetPlayerState(PlayerState.Running);
    }      

    private IEnumerator RemoveEffectAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        RemoveEffect();
    }
    public virtual void DisableEffect()
    {
        RemoveEffect();
    }
    private void UpdatePlayerState()
    {
        switch (type)
        {
            case ItemType.Invisibility:
                gameStateSO.SetPlayerState(PlayerState.Invisible);
                break;
            case ItemType.SpeedBoost:
                gameStateSO.SetPlayerState(PlayerState.SpeedBoost);
                break;
            case ItemType.Shield:
                gameStateSO.SetPlayerState(PlayerState.Shielded);
                break;
            case ItemType.Magnet:
                gameStateSO.SetPlayerState(PlayerState.Magnet);
                break;
        }
    }
}
