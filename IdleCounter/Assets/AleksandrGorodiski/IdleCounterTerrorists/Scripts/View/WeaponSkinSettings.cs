using UnityEngine;

public class WeaponSkinSettings : MonoBehaviour
{
    [SerializeField]
    private Transform[] bulletNodes;
    [SerializeField]
    private Transform[] catridgeCaseNodes;
    [SerializeField]
    private GameObject[] muzzleFlashes;

    public Transform[] GetBulletNode()
    {
        return bulletNodes;
    }
    public Transform[] GetCatridgeNode()
    {
        return catridgeCaseNodes;
    }

    public GameObject[] GetMuzzleFlashes()
    {
        return muzzleFlashes;
    }
}
