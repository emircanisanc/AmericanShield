using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Meteor : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody rb;
    Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveDirection(Vector3 direction)
    {
        this.direction = direction;
        gameObject.SetActive(true);
    }

    void Update()
    {
        rb.velocity = direction * moveSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.ApplyDamage(200);
            }
            gameObject.SetActive(false);
        }    
    }
}
