using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MainHealth
{
    [SerializeField] private bool destroyable = false;

    public override void TakeDamage(int damage, Vector3 point, Vector3 hitDirection)
    {
        if (destroyable)
        {
            base.TakeDamage(damage, point, hitDirection);

        }
    }
}
