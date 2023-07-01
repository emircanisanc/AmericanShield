using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{

    # region Singleton

    public static InGameUI Instance { get; private set; }

    # endregion

    [SerializeField] GameObject winGameUI;
    [SerializeField] GameObject mainUI;
    [SerializeField] SkillButton[] skillButtons;
    [SerializeField] int expOnLevelEnd;
    [SerializeField] Image levelUpBar;
    [SerializeField] GameObject nextLevelBtn;
    [SerializeField] GameObject levelUpText;
    [SerializeField] WeaponListSO weaponList;

    void Awake()
    {
        Instance = this;

        foreach(var skillButton in skillButtons)
        {
            skillButton.gameObject.SetActive(false);
        }    
    }

    void Start()
    {
        FindObjectOfType<LevelManager>().OnLevelEnd += ShowWinGameUI;
        ShowMainUI();
    }

    public void AddSkill(int index, UnityAction action, Sprite image)
    {
        skillButtons[index].SetSkill(action, image);
        skillButtons[index].gameObject.SetActive(true);
    }

    private void ShowWinGameUI()
    {
        winGameUI.SetActive(true);
        
        string weaponName = SaveLoadManager.CurrentWeaponName();
        int currentExp = SaveLoadManager.WeaponExp(weaponName);
        levelUpBar.fillAmount = (float)currentExp / 100;
        int targetExp = currentExp + expOnLevelEnd;
        targetExp = Mathf.Clamp(targetExp, 0, 100);
        if (targetExp == 100)
        {
            WeaponSO weaponSO = weaponList.weapons.Find(x => x.weaponName == weaponName);
            int currentLevel = SaveLoadManager.WeaponLevel(weaponName);
            if (weaponSO.maxLevel == currentLevel)
            {
                SaveLoadManager.SaveWeaponExp(weaponName, targetExp);
                ShowNextLevelButton(false);
                return;
            }
            else
            {
                SaveLoadManager.SaveWeaponExp(weaponName, 0);
                SaveLoadManager.SaveWeaponLevel(weaponName, currentLevel + 1);
            }
        }
        else
        {
            SaveLoadManager.SaveWeaponExp(weaponName, targetExp);
        }
        levelUpBar.DOFillAmount((float)targetExp / 100f, 1.5f).OnComplete(() => ShowNextLevelButton(targetExp == 100));

    }

    private void ShowNextLevelButton(bool isWeaponLevelUp)
    {
        if (isWeaponLevelUp)
        {
            levelUpText.SetActive(true);
        }
        nextLevelBtn.SetActive(true);
    }

    private void ShowMainUI()
    {
        mainUI.SetActive(true);
    }

    public void OpenLevelSelection()
    {
        SceneManager.LoadScene("WeaponSelect");
    }
}
