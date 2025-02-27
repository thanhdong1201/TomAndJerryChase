using UnityEngine;

public class ShieldPowerUp : PowerUpBase
{
    [SerializeField] protected ParticleSystem destroyParticle;
    public override ItemType type => ItemType.Shield;
    protected override void ApplyEffect()
    {
        base.ApplyEffect();
    }

    protected override void RemoveEffect()
    {
        base.RemoveEffect();
    }
    public override void DisableEffect()
    {
        base.DisableEffect();   
        destroyParticle.Play();
    }
}
