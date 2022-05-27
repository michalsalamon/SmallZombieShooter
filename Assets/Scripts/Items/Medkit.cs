using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : CollectibleItem
{
    [Header("Medkit settings")]
    [SerializeField] private int amountOfHeal = 2;
    [SerializeField] private float healRange = 1;

    public override void UseItemPrimary()
    {
        //get target (in this case owner)
        InterfaceDamagable intDam = transform.root.GetComponent<InterfaceDamagable>();

        if (intDam != null)
        {
            if (intDam.GainHealth(amountOfHeal))
            {
                Destroy(gameObject);
            }
        }

    }

    public override void UseItemSecondary()
    {
        Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, healRange))
        {
            InterfaceDamagable intDmg = hit.transform.GetComponent<InterfaceDamagable>();
            if (intDmg != null)
            {
                if (intDmg.GainHealth(amountOfHeal))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
