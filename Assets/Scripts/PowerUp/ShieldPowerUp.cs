using UnityEngine;

public class ShieldPowerUp : PowerUpBase
{
    public override ItemType type => ItemType.Shield;
    protected override void ApplyEffect()
    {
        base.ApplyEffect();
    }

    protected override void RemoveEffect()
    {
        base.RemoveEffect();
    }
}
