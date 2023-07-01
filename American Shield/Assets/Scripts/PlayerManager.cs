using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour, IDamageable
{   
    public static Action OnPlayerDie;

    bool isDead;

    public void ApplyDamage(int damage)
    {
        if (isDead)
            return;
        
        isDead = true;
        OnPlayerDie?.Invoke();
    }
}