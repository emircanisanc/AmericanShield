using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapButton : MonoBehaviour, IDamageable
{

    public List<Animator> trapAnimator;
    bool isActive = true;

    public void ApplyDamage(int damage)
    {
        if (isActive)
        {
            isActive = false;
            foreach (Animator animator in trapAnimator)
            {
                animator.SetTrigger("isPush");
            }

        }
    }
}
