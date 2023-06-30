using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapButton : MonoBehaviour, IDamageable
{
    [SerializeField] Animator trapAnimator;
    bool isActive = true;

    public void ApplyDamage(int damage)
    {
        if (isActive)
        {
            isActive = false;
            trapAnimator.SetTrigger("isPush");
        }
    }
}
