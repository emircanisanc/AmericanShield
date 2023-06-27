using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponList")]
public class WeaponListSO : ScriptableObject
{
    public List<WeaponSO> weapons;
}
