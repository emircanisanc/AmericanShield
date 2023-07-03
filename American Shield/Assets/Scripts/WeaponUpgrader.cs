using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class WeaponUpgrader : MonoBehaviour
{
    [SerializeField] WeaponListSO weaponList;
    [SerializeField] Transform weaponSpawnPoint;
    [SerializeField] Transform stoneSpawnTransform;
    [SerializeField] GameObject upgradeBtn;
    [SerializeField] GameObject nextBtn;

    Transform stoneTransform;
    Vector3 stoneTargetPoint;

    void Awake()
    {
        string weaponName = SaveLoadManager.CurrentWeaponName();
        GameObject prefab = weaponList.weapons.Find(x => x.weaponName == weaponName).modelPrefab;
        GameObject obj = Instantiate(prefab, weaponSpawnPoint);

        int weaponLevel = SaveLoadManager.WeaponLevel(weaponName);
        WeaponStoneSpawner stoneSpawner = obj.GetComponent<WeaponStoneSpawner>();
        stoneSpawner.LoadStones(weaponLevel - 1);
        stoneTransform = stoneSpawner.stones[weaponLevel - 1].transform;
        stoneTargetPoint = stoneTransform.position;

        stoneTransform.position = stoneSpawnTransform.position;
        stoneTransform.gameObject.SetActive(true);

    }

    private void OnUpgradeCompleted()
    {
        nextBtn.SetActive(true);
    }

    public void Upgrade()
    {
        upgradeBtn.SetActive(false);
        stoneTransform.DOMove(stoneTargetPoint, 1.5f).SetEase(Ease.Flash).OnComplete(() => OnUpgradeCompleted());
    }

    public void Next()
    {
        SceneManager.LoadScene("WeaponSelect");
    }
}
