using UnityEngine;

public class SpeedBoostPowerUp : PowerUpBase
{
    [SerializeField] private float speedMultiplier = 1.5f;

    [Header("Broadcast from Events")]
    [SerializeField] private BoolEventChannelSO applyMotionBlurEventChannel;

    public override ItemType type => ItemType.SpeedBoost;

    protected override void ApplyEffect()
    {
        base.ApplyEffect();
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.ChangeSpeed(speedMultiplier, duration);
        applyMotionBlurEventChannel?.RaiseEvent(true);
    }
    protected override void RemoveEffect()
    {
        base.RemoveEffect();
        applyMotionBlurEventChannel?.RaiseEvent(false);
    }
}
