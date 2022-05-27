using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//base class for items in world

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class ItemBreakable : MonoBehaviour, InterfaceDamagable
{
    [SerializeField] private bool isAffectedByGravity = true;
    //[SerializeField] private bool isPuschable = true;

    [Header("Breakable settings")]
    [SerializeField] private bool isBreakable = false;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int armor = 2;
    private int currentHealth;

    private Rigidbody RB;

    private UnityEvent objectDestroyEvent = new UnityEvent();
    public UnityEvent ObjectDestroyEvent { get { return objectDestroyEvent; } }

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();

        if (RB != null)
        {
            RB.useGravity = isAffectedByGravity;
        }

        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage, Vector3 hitDirection, Vector3 point)
    {
        if (isBreakable)
        {
            int hit = damage - armor;
            if (hit > 0)
            {
                currentHealth -= hit;
                if (currentHealth <= 0)
                {
                    BreakeItem();
                }
            }
        }
    }

    public virtual bool GainHealth(int healthAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth + healthAmount, 0, maxHealth);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void BreakeItem()
    {
        objectDestroyEvent.Invoke();

        Destroy(gameObject);
    }
}