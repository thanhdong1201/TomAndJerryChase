using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainLifePowerUp : PowerUpBase
{
    [SerializeField] PlayerDataSO playerDataSO;
    public override PowerUpType type => PowerUpType.GainLife;
    protected override void ApplyEffect()
    {
        //playerDataSO.GainLife();
    }
    protected override void RemoveEffect()
    {

    }
}
