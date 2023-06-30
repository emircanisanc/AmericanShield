using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.ApplyDamage(500);
            }
        }    
    }
}
