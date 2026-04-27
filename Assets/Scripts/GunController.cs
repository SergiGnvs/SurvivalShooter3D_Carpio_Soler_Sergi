using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField] GunData gunData;

    [SerializeField] GameObject user;
    [SerializeField] Transform barrel;

    int curAmmo;

    bool canShoot = true;

    void Awake()
    {
        curAmmo = gunData.maxAmmo;

    }



    public void Shoot(EnemyController target)
    {
        if (canShoot == false) return;

        Debug.Log("BANG!!");

        RaycastHit hit;
        Vector3 origin = barrel.transform.position;
        Vector3 direction = user.transform.forward;
        Vector3 endPoint;

        if(Physics.Raycast(origin, direction, out hit, gunData.range))
        {
            endPoint = hit.point;

            if (hit.transform.GetComponent<EnemyController>())
            {
                hit.transform.GetComponent<EnemyController>().GetDamaged(gunData.damage);
            }
        }
        else
        {
            endPoint = origin + direction * gunData.range;

        }

        --curAmmo;

        Debug.Log("Ammo Wallahi: " + curAmmo);

        if (curAmmo == 0) Reload();
        else StartCoroutine(WaitFireRate());

    }

    public void Reload()
    {
        canShoot = false;
        StartCoroutine(Reloading());
    }

    public float GetRange() { return gunData.range; }

    public void SwitchWeapon() 
    { 

    }

    IEnumerator WaitFireRate()
    {
        canShoot = false;
        yield return new WaitForSeconds(gunData.fireRate);
        canShoot = true;
    }
    IEnumerator Reloading()
    {
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(gunData.reloadTime);
        curAmmo = gunData.maxAmmo;
        canShoot = true;
        Debug.Log("Reloaded");
    }

}
