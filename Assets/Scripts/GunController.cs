using System.Collections;
using UnityEngine;
using UnityEditor;

public class GunController : MonoBehaviour, IWeapon
{

    [SerializeField] GunData gunData;
    [SerializeField] TrailData trailData;
    [SerializeField] LineData lineData;

    [SerializeField] GameObject user;
    [SerializeField] Transform barrel;

    float Damage;
    float ReloadTime;
    float Range;
    float FireRate;
    int MaxAmmo;
    int CurrentAmmo;

    bool CanShoot = true;

    void Awake()
    {
        Damage = gunData.Damage;
        ReloadTime = gunData.ReloadTime;
        Range = gunData.Range;
        FireRate = gunData.FireRate;
        MaxAmmo = gunData.MaxAmmo;
        CurrentAmmo = gunData.MaxAmmo;

    }



    public void Shoot(EnemyController target)
    {
        if (!CanShoot) return;

        CanShoot = false;

        Debug.Log("BANG!!");


        Vector3 origin = barrel.position;
        Vector3 direction = (target.transform.position - origin).normalized;

        RaycastHit hit;


        if (Physics.Raycast(origin, direction, out hit, gunData.Range))
        {
            EnemyController enemy = hit.transform.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.GetDamaged(gunData.Damage);
            }

            Transform[] puntos = new Transform[5];
            puntos[0] = barrel;
            puntos[1] = hit.transform;


        }

        CurrentAmmo--;

        Debug.Log("Ammo: " + CurrentAmmo);

        if (CurrentAmmo <= 0)
        {
            Reload();
        }
        else
        {
            StartCoroutine(WaitFireRate());
        }
    }

    public void Reload()
    {
        //CanShoot = false;
        StartCoroutine(Reloading());
    }

    public float GetRange()
    {
        return Range;
    }

    public void SwitchWeapon() 
    {
        Reload();
    }

    IEnumerator WaitFireRate()
    {
        CanShoot = false;
        yield return new WaitForSeconds(gunData.FireRate);
        CanShoot = true;
    }
    IEnumerator Reloading()
    {
        Debug.Log("Reloading...");
        CanShoot = false;
        yield return new WaitForSeconds(gunData.ReloadTime);
        CurrentAmmo = gunData.MaxAmmo;
        CanShoot = true;
        Debug.Log("Reloaded");
    }

}
