using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour, IDamageable
{   
    public static Action OnPlayerDie;

    bool isDead;

    public bool ApplyDamage(int damage)
    {
        if (isDead)
            return false;
        
        isDead = true;
        OnPlayerDie?.Invoke();
        return true;
    }
}