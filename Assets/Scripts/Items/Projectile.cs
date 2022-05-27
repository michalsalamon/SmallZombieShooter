using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 40;
    [SerializeField] private float range = 15;
    [SerializeField] private int damage = 1;

    [SerializeField] LayerMask collisionMask;

    private void Start()
    {
        Destroy(gameObject, range / speed);
        //if gun is inside other object we need to check collision on spawn
        Collider[] spawnColliders = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
        if (spawnColliders.Length > 0)
        {
            OnHitObject(spawnColliders[0], transform.position);
        }
    }

    private void FixedUpdate()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollision(moveDistance);
        transform.Translate(moveDistance * Vector3.forward);   
    }

    private void CheckCollision(float distance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }

    private void OnHitObject(Collider col, Vector3 hitPoint)
    {
        InterfaceDamagable damagableObject = col.GetComponent<InterfaceDamagable>();
        if (damagableObject != null)
        {
            Vector3 hitDirection = hitPoint - transform.position;
            damagableObject.TakeDamage(damage, hitPoint, hitDirection);
        }
        Destroy(gameObject);
    }
}
