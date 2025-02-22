using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class SpeedBoostPowerUp : PowerUpBase
{
    [SerializeField] private float speedMultiplier = 1.5f;

    [Header("Broadcast from Events")]
    [SerializeField] private BoolEventChannelSO applyMotionBlurEventChannel;

    public override PowerUpType type => PowerUpType.SpeedBoost;

    protected override void ApplyEffect()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.ChangeSpeed(speedMultiplier,  duration);
        applyMotionBlurEventChannel?.RaiseEvent(true);
    }
    protected override void RemoveEffect()
    {
        applyMotionBlurEventChannel?.RaiseEvent(false);
    }
}
