using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    [System.Serializable]
    private class UnlockWeapon
    {
        public int unlockLevel;
        public string weaponName;
    }



    [SerializeField] WeaponListSO weaponList;
    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject unlockWeaponPanel;
    [SerializeField] Image unlockWeaponImage;
    [SerializeField] float distanceWeaponSlector = 1f;
    [SerializeField] List<UnlockWeapon> unlockWeapons;


    Vector3 camPos;
    int currentSelected;
    int currentCursor;
    int weaponCount;

    void Awake()
    {
        int i = 0;
        string currentWeapon = SaveLoadManager.CurrentWeaponName();
        foreach (var weaponSO in weaponList.weapons)
        {
            if (weaponSO.IsUnlocked)
            {
                if (weaponSO.weaponName == currentWeapon)
                {
                    currentSelected = i;
                    currentCursor = currentSelected;
                }
            }
            else
            {

            }
            Instantiate(weaponSO.modelPrefab, transform).transform.localPosition = new Vector3(i * distanceWeaponSlector, 0, 0);
            i++;
        }
        var x = currentSelected * distanceWeaponSlector;
        camPos = cameraTransform.position;
        cameraTransform.position = new Vector3(x, camPos.y, camPos.z);
        weaponCount = weaponList.weapons.Count;

        int currentLevel = SaveLoadManager.CurrentLevel();
        foreach (var unlockWeapon in unlockWeapons)
        {
            if (unlockWeapon.unlockLevel == currentLevel)
            {
                SaveLoadManager.SetWeaponUnlocked(unlockWeapon.weaponName);
                WeaponSO weapon = weaponList.weapons.Find(x => x.weaponName == unlockWeapon.weaponName);
                unlockWeaponImage.sprite = weapon.unlockImage;
                unlockWeaponPanel.SetActive(true);
                return;
            }
        }
    }

    public void SelectRight()
    {
        if (currentCursor + 1 < weaponCount)
        {
            currentCursor++;
            Vector3 targetPos = cameraTransform.position + new Vector3(distanceWeaponSlector, 0, 0);
            cameraTransform.DOMove(targetPos, 0.5f);
            CheckStartButtonActive();
        }
    }

    public void SelectLeft()
    {
        if (currentCursor - 1 >= 0)
        {
            currentCursor--;
            Vector3 targetPos = cameraTransform.position + new Vector3(-distanceWeaponSlector, 0, 0);
            cameraTransform.DOMove(targetPos, 0.5f);
            CheckStartButtonActive();
        }
    }

    private void CheckStartButtonActive()
    {
        startBtn.SetActive(weaponList.weapons[currentCursor].IsUnlocked);
    }

    public void SelectAndStart()
    {
        SaveLoadManager.SaveCurrentWeapon(weaponList.weapons[currentCursor].weaponName);
        SceneManager.LoadScene("Level " + SaveLoadManager.CurrentLevel());
    }

}
