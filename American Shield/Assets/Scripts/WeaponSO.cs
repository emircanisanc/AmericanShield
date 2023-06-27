using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponSO : ScriptableObject
{
    public GameObject prefab;
    public GameObject modelPrefab;
    public string weaponName = "Shield";
    public bool IsUnlocked { get{ return  SaveLoadManager.WeaponIsUnclocked(weaponName);} }


}
