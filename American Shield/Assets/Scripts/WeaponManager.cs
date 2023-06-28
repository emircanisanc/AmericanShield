using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponListSO weaponList;
    [SerializeField] public bool SpawnFromLoad;
    [SerializeField] public int SpawnIndex;

    void Awake()
    {
        if (SpawnFromLoad)
        {
            string weaponName = SaveLoadManager.CurrentWeaponName();
            GameObject currentPrefab = weaponList.weapons.Find(x => x.weaponName == weaponName).prefab;

            Instantiate(currentPrefab, transform).transform.localPosition = Vector3.zero;
        }
        else
        {
            GameObject prefab = weaponList.weapons[SpawnIndex].prefab;
            Instantiate(prefab, transform).transform.localPosition = Vector3.zero;
        }

    }
}
