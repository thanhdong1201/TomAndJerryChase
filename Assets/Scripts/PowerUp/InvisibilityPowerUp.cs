using UnityEngine;

public class InvisibilityPowerUp : PowerUpBase
{
    [SerializeField] Material playerMaterial;
    [SerializeField] Material invisibleMaterial;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    private Material defaultMaterial;

    public override ItemType type => ItemType.Invisibility;
    protected override void ApplyEffect()
    {
        base.ApplyEffect();
        skinnedMeshRenderer.material = invisibleMaterial;
    }

    protected override void RemoveEffect()
    {
        base.RemoveEffect();
        skinnedMeshRenderer.material = playerMaterial;
    }
}
