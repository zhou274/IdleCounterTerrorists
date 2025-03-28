using UnityEngine;

public class UnitSkinSettings : MonoBehaviour
{
    public Transform unitTransformNode;
    public Transform weaponNode;
    public Renderer meshRenderer;
    [HideInInspector]
    public Material normalMaterial;
    public Material hitMaterial;
    public ParticleSystem bloodParticles;
    public UnitAnimation unitAnimation;
    public HealthBarController healthBarController;
    public HealthBarController reloadBarController;

    private void Start()
    {
        normalMaterial = meshRenderer.material;
    }

    public Transform GetWeaponNode()
    {
        return weaponNode;
    }

    public Transform GetUnitTransformNode()
    {
        return unitTransformNode;
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}