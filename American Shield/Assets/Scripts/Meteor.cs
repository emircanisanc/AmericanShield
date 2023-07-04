using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Meteor : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] int damage = 200;
    [SerializeField] GameObject gfx;
    [SerializeField] GameObject particle;
    [SerializeField] float areaRadius = 2f;

    Rigidbody rb;
    Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveDirection(Vector3 direction)
    {
        this.direction = direction;
        gfx.SetActive(true);
        particle.SetActive(false);
        gameObject.SetActive(true);
        enabled=true;
    }

    void Update()
    {
        rb.velocity = direction * moveSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Weapon"))
        {
            rb.velocity = Vector3.zero;
            enabled=false;
            gfx.SetActive(false);
            particle.SetActive(true);

            Collider[] colliders = Physics.OverlapSphere(transform.position, areaRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                    continue;
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.ApplyDamage(damage);
                }
            }

            Invoke("CloseObj", 1f);
        }
    }


    private void CloseObj()
    {
        gameObject.SetActive(false);
    }
}
