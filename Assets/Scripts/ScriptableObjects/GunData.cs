using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public float damage;
    public float range;
    public int maxAmmo;
    public float reloadTime;
    public float fireRate;
}
