using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterfaceDamagable
{ 
    void TakeDamage(int damage, Vector3 hitDirection, Vector3 pointHitted);

    bool GainHealth(int healthAmount);
}
