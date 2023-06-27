using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponSO : ScriptableObject
{
    public GameObject prefab;
    public string weaponName = "Shield";
    public bool isUnlocked;

    void OnValidate()
    {
        isUnlocked = SaveLoadManager.WeaponIsUnclocked(weaponName);
    }

}
