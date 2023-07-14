using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tnt : MonoBehaviour, IDamageable
{
    [SerializeField] int damage = 200;
    [SerializeField] GameObject[] toggleObjects;
    [SerializeField] float destroyTime = 1.3f;
    [SerializeField] AudioClip explosionClip;
    Collider[] collidersToClose;
    [SerializeField] float areaRadius = 5f;

    void Awake()
    {
        collidersToClose = GetComponents<Collider>();
    }

    bool isDone;

    public bool ApplyDamage(int damage)
    {
        if (isDone)
            return false;

        isDone = true;
        AudioManager.PlayClip(explosionClip, transform.position);
        foreach (var coll in collidersToClose)
            coll.enabled = false;
        foreach (var obj in toggleObjects)
        {
            obj.SetActive(!obj.activeSelf);
        }

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

        Destroy(gameObject, destroyTime);
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isDone && (other.CompareTag("Player") || other.CompareTag("Weapon")))
        {
            ApplyDamage(damage);
        }
    }
}
