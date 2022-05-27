using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainHealth : MonoBehaviour, InterfaceDamagable
{
    [SerializeField] private int MaxHealth = 5;
    private int currentHealth;

    public virtual void Start()
    {
        currentHealth = MaxHealth;
    }

    public virtual void TakeDamage(int damage, Vector3 hitDirection, Vector3 point)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual bool GainHealth(int healthAmount)
    {
        if (currentHealth >= MaxHealth)
        {
            return false;
        }
        else
        {
            currentHealth = Mathf.Clamp(currentHealth + healthAmount, 0, MaxHealth);
            Debug.Log(currentHealth);
            return true;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
    