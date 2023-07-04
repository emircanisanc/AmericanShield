using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapButton : MonoBehaviour, IDamageable
{
    [SerializeField] AudioClip hitClip;
    public List<Animator> trapAnimator;
    bool isActive = true;

    public bool ApplyDamage(int damage)
    {
        if (isActive)
        {
            // animator

            isActive = false;
            foreach (Animator animator in trapAnimator)
            {
                animator.SetTrigger("isPush");
            }
            AudioManager.PlayClip(hitClip, transform.position);
            return false;
        }
        return false;
    }
}
