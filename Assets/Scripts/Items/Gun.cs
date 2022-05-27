using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : CollectibleItem
{
    [Header ("Gun settings")]
    [SerializeField] private float msBetweenShoot = 100;

    [SerializeField] private Projectile ammoType;
    [SerializeField] private Transform muzzle;
    public Transform Muzzle
    { get { return muzzle; } }

    private float nextShootTime;
    
    public override void UseItemPrimary()
    {
        if (nextShootTime < Time.time)
        {
            nextShootTime = Time.time + msBetweenShoot / 1000;
            Instantiate(ammoType, muzzle.position, muzzle.rotation);
        }
        
    }
}
