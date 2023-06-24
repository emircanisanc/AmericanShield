using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamager
{
    public bool IsAttacking();
    public void TurnAttackBack();
    public void BlockAttack();
}
