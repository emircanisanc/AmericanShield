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
    [SerializeField] GameObject loseGameUI;
    [SerializeField] GameObject mainUI;
    [SerializeField] SkillButton[] skillButtons;
    [SerializeField] int expOnLevelEnd;
    [SerializeField] Image levelUpBar;
    [SerializeField] GameObject weaponIsMaxLevelText;
    [SerializeField] GameObject levelUpParent;
    [SerializeField] GameObject nextLevelBtn;
    [SerializeField] GameObject levelUpText;
    [SerializeField] GameObject openWeaponCraftBtn;
    [SerializeField] WeaponListSO weaponList;
    [SerializeField] AudioClip loseGameClip;
    [SerializeField] AudioClip winGameClip;
    [SerializeField] GameObject wonEffect;
    [SerializeField] GameObject loseEffect;

    void Awake()
    {
        Instance = this;

        foreach (var skillButton in skillButtons)
        {
            skillButton.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        LevelManager.OnLevelEnd += ShowWinGameUI;
        PlayerManager.OnPlayerDie += ShowLoseGameUI;
        ShowMainUI();
    }

    void OnDisable()
    {
        LevelManager.OnLevelEnd -= ShowWinGameUI;
        PlayerManager.OnPlayerDie -= ShowLoseGameUI;
    }

    public void AddSkill(int index, UnityAction action, Sprite image)
    {
        skillButtons[index].SetSkill(action, image);
        skillButtons[index].gameObject.SetActive(true);
    }

    private void ShowLoseGameUI()
    {
        AudioManager.PlayClipAtCamera(loseGameClip);
        loseGameUI.SetActive(true);
    }

    private void ShowWinGameUI()
    {
        winGameUI.SetActive(true);
        if (wonEffect)
            wonEffect.SetActive(true);
        AudioManager.PlayClipAtCamera(winGameClip);

        string weaponName = SaveLoadManager.CurrentWeaponName();
        WeaponSO weaponSO = weaponList.weapons.Find(x => x.weaponName == weaponName);
        int currentLevel = SaveLoadManager.WeaponLevel(weaponName);
        if (weaponSO.maxLevel == currentLevel)
        {
            weaponIsMaxLevelText.SetActive(true);
            levelUpParent.SetActive(false);
            ShowNextLevelButton();
            return;
        }


        int currentExp = SaveLoadManager.WeaponExp(weaponName);
        levelUpBar.fillAmount = (float)currentExp / 100;
        int targetExp = currentExp + expOnLevelEnd;
        targetExp = Mathf.Clamp(targetExp, 0, 100);
        if (targetExp == 100)
        {
            SaveLoadManager.SaveWeaponExp(weaponName, 0);
            SaveLoadManager.SaveWeaponLevel(weaponName, currentLevel + 1);
            levelUpBar.DOFillAmount((float)targetExp / 100f, 1.5f).OnComplete(() => OnWeaponLevelUp());
        }
        else
        {
            SaveLoadManager.SaveWeaponExp(weaponName, targetExp);
            levelUpBar.DOFillAmount((float)targetExp / 100f, 1.5f).OnComplete(() => ShowNextLevelButton());
        }
    }

    private void OnWeaponLevelUp()
    {
        levelUpText.SetActive(true);
        openWeaponCraftBtn.SetActive(true);
    }

    private void ShowNextLevelButton()
    {
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

    public void OpenWeaponCraft()
    {
        SceneManager.LoadScene("WeaponUpgrade");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
