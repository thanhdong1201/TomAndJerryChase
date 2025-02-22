using UnityEngine;

public class ShieldPowerUp : PowerUpBase
{
    [SerializeField] private GameObject shieldVisual;

    public override PowerUpType type => PowerUpType.Shield;
    protected override void ApplyEffect()
    {
        shieldVisual.SetActive(true);
    }

    protected override void RemoveEffect()
    {
        shieldVisual.SetActive(false);
    }
}
