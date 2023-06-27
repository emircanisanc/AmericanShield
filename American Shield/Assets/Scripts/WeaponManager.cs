using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponListSO weaponList;

    void Awake()
    {
        string weaponName = SaveLoadManager.CurrentWeaponName();
        GameObject currentPrefab = weaponList.weapons.Find(x => x.weaponName == weaponName).prefab;

        Instantiate(currentPrefab, transform).transform.localPosition = Vector3.zero;
    }
}
