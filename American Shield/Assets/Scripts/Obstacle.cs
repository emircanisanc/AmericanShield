using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject[] toggleObjects;
    [SerializeField] float destroyTime = 1.3f;
    Collider[] collidersToClose;

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
        foreach (var coll in collidersToClose)
            coll.enabled = false;
        foreach (var obj in toggleObjects)
        {
            obj.SetActive(!obj.activeSelf);
        }
        Destroy(gameObject, destroyTime);
        return false;
    }
}
