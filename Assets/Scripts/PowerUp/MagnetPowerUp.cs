using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPowerUp : PowerUpBase
{
    public override PowerUpType type => PowerUpType.Magnet;
    protected override void ApplyEffect()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {

        }
    }
    protected override void RemoveEffect()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {

        }
    }
}
