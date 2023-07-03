using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject[] toggleObjects;
    [SerializeField] float destroyTime = 1.3f;

    bool isDone;

    public void ApplyDamage(int damage)
    {
        if (isDone)
            return;
        
        isDone = true;
        foreach (var obj in toggleObjects)
        {
            obj.SetActive(!obj.activeSelf);
        }
        Destroy(gameObject, destroyTime);
    }
}
