using UnityEngine;

public class InvisibilityPowerUp : PowerUpBase
{
    [SerializeField] Material playerMaterial;
    [SerializeField] Material invisibleMaterial;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

    private Material defaultMaterial;
    public override PowerUpType type => PowerUpType.Invisibility;
    protected override void ApplyEffect()
    {
        skinnedMeshRenderer.material = invisibleMaterial;
    }

    protected override void RemoveEffect()
    {
        skinnedMeshRenderer.material = playerMaterial;
    }
}
