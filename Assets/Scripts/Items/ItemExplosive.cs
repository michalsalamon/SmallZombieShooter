using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExplosive : MonoBehaviour
{
    [Header("Explosive settings")]
    [SerializeField] private bool isExplosive = false;
    [SerializeField] private int explosionDamage = 10;
    [SerializeField] private float explosionRadius = 1.5f;
    [Range (0, 10)][SerializeField] private float explosionForce = 10;

    private ItemBreakable itemBreakable;

    private void Awake()
    {
        itemBreakable = GetComponent<ItemBreakable>();
        if (itemBreakable != null)
        {
            itemBreakable.ObjectDestroyEvent.AddListener(Explode);
        }
    }

    private void Explode()
    {

        Collider[] colls;
        colls = Physics.OverlapSphere(transform.position, explosionRadius);
        //todo obstacle can protect agaunst explosion

        foreach (Collider col in colls)
        {
            if (col.transform != transform)
            {
                //add explosion force to object witch can be hit by it
                Rigidbody hitRB = col.GetComponent<Rigidbody>();
                if (hitRB != null)
                {
                    hitRB.AddExplosionForce(10000 * explosionForce, transform.position, explosionRadius);
                }
                InterfaceDamagable intDam = col.GetComponent<InterfaceDamagable>();
                if (intDam != null)
                {
                    //add dmg to hit objects if they can take it
                    intDam.TakeDamage(explosionDamage, col.transform.position - transform.position, col.transform.position);
                }
            }
        }
    }
}
