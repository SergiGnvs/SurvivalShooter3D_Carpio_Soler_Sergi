using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public float Damage;
    public float Range;
    public int MaxAmmo;
    public float ReloadTime;
    public float FireRate;
}
