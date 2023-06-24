using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour, IDamager
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public bool IsAttacking()
    {
        return true;
    }

    public void TurnAttackBack()
    {
        rb.velocity = -rb.velocity;
    }

    public void BlockAttack()
    {
        Destroy(gameObject);   
    }
}
