using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [Header("SomeReference")]
    [SerializeField] private GameStateSO gameStateSO;
    [SerializeField] private PlayerDataSO playerDataSO;

    [Header("Effect")]
    [SerializeField] private ParticleSystem collectCoinEffect;
    [SerializeField] private ParticleSystem collectItemEffect;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem stunEffect;

    [Header("SFX")]
    [SerializeField] private AudioCueSO audioCueSO;
    [SerializeField] private AudioClip collectClip;
    [SerializeField] private AudioClip getDamageClip;
    [SerializeField] private AudioClip hitObstacleClip;

    [Header("Broadcasting from Events")]
    [SerializeField] private VoidEventChannelSO cameraShakeHardEvent;
    [SerializeField] private VoidEventChannelSO cameraShakeSoftEvent;
    [SerializeField] private VoidEventChannelSO applyVintageEvent;

    private PlayerController playerController;
    private PlayerInventory playerInventory;

    private float lastTriggerTime = 0f;
    private const float triggerCooldown = 0.01f;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerInventory = GetComponent<PlayerInventory>();
    }
    private void Update()
    {
        if (gameStateSO.CurrentGameState == GameState.GameOver) return;

        playerDataSO.currentDistance = transform.position.z;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastTriggerTime < triggerCooldown) return;
        lastTriggerTime = Time.time;

        if (gameStateSO.CurrentPlayerState == PlayerState.Dead) return;

        switch (other.tag)
        {
            case "Collectible":
                if (other.TryGetComponent(out Collectible collectible))
                {
                    HandleCollectibleCollision(collectible);
                }
                break;
            case "Obstacle":
                HandleObstacleCollision();
                break;
            case "StumbleObstacle":
                HandleStumbleObstacleCollision();
                break;
            case "Damageable":
                TakeDamage(transform.position);
                break;
        }
    }
    private void HandleCollectibleCollision(Collectible collectible)
    {

        collectible.Collect();
        if (collectible.itemType == ItemType.Coin)
        {
            collectCoinEffect.Play();
            playerDataSO.AddCurrentCoin(1);
            audioCueSO.PlaySFX(collectClip);
        }
        else
        {
            collectItemEffect.Play();
            playerInventory.AddPowerUp(collectible.itemType);
        }

    }
    private void HandleObstacleCollision()
    {
        if (gameStateSO.CurrentPlayerState == PlayerState.Shielded)
        {
            return;
        }
        else
        {
            audioCueSO.PlaySFX(hitObstacleClip);
            stunEffect.Play();
            cameraShakeHardEvent.RaiseEvent();
            playerController.Die(new Vector3(0f, 2f, -3f));
        }
    }
    private void HandleStumbleObstacleCollision()
    {      
        if (gameStateSO.CurrentPlayerState == PlayerState.Shielded || gameStateSO.CurrentPlayerState == PlayerState.Invisible || gameStateSO.CurrentPlayerState == PlayerState.SpeedBoost)
        {
            return;
        }
        else
        {
            audioCueSO.PlaySFX(hitObstacleClip);
            stunEffect.Play();
            cameraShakeSoftEvent.RaiseEvent();
            playerController.GetHit();
            gameStateSO.TriggerObstacleCollision();
        }
    }
    public void TakeDamage(Vector3 damagePosition)
    {
        audioCueSO.PlaySFX(getDamageClip);
        hitEffect.transform.position = damagePosition;
        hitEffect.Play();      

        if (gameStateSO.CurrentPlayerState == PlayerState.Shielded)
        {
            cameraShakeSoftEvent.RaiseEvent();
        }
        else
        {
            cameraShakeHardEvent.RaiseEvent();
            applyVintageEvent.RaiseEvent();
            playerController.Die(new Vector3(0f, 6f, 8f));
        }
    }
}
