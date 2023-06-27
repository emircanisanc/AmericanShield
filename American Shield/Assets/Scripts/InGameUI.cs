using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameUI : MonoBehaviour
{

    # region Singleton

    public static InGameUI Instance { get; private set; }

    # endregion

    [SerializeField] GameObject winGameUI;
    [SerializeField] GameObject mainUI;
    [SerializeField] SkillButton[] skillButtons;

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
    }

    private void ShowMainUI()
    {
        mainUI.SetActive(true);
    }
}
