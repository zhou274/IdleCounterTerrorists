using UnityEngine;

public class PlayerShopView : UnitView
{
    public float _rotSpeed = 10f;
    private float _scale = 50f;

    private void Start()
    {
        SetUnitRotation(180f);
        SetSkinScale(_scale);
        SetSkinPosition(_scale);
    }

    void SetSkinScale(float value)
    {
        skinUnit.transform.localScale = new Vector3(value, value, value);
    }

    void SetSkinPosition(float value)
    {
        skinUnit.transform.localPosition = new Vector3(0f, -value, 0f);
    }

    public override void Update()
    {

    }

    public override void PlayAnimationOnStart()
    {
        unitSkinSettings.unitAnimation.PlayShopAnimation();
    }

    public override void SetLayerOnStart()
    {
        unitSkinSettings.meshRenderer.gameObject.layer = 11;
        //unitSkinSettings.GetComponent<Renderer>().gameObject.layer = 11;
    }

    public void PlayAnimationOnHire()
    {
        unitSkinSettings.unitAnimation.PlayAnimationOnHire();
    }
}