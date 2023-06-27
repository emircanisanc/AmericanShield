using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class WeaponSelector : MonoBehaviour
{
    [SerializeField] WeaponListSO weaponList;
    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject startBtn;

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
            Instantiate(weaponSO.modelPrefab, transform).transform.localPosition = new Vector3(i * 0.7f, 0, 0);
            i++;
        }
        var x = currentSelected * 0.7f;
        camPos = cameraTransform.position;
        cameraTransform.position = new Vector3(x, camPos.y, camPos.z);
        weaponCount = weaponList.weapons.Count;
    }

    public void SelectRight()
    {
        if (currentCursor + 1 < weaponCount)
        {
            currentCursor++;
            Vector3 targetPos = camPos + new Vector3(0.7f, 0, 0);
            cameraTransform.DOMove(targetPos, 0.5f);
        }
    }

    public void SelectLeft()
    {
        if (currentCursor - 1 >= 0)
        {
            currentCursor--;
            Vector3 targetPos = camPos + new Vector3(-0.7f, 0, 0);
            cameraTransform.DOMove(targetPos, 0.5f);
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
