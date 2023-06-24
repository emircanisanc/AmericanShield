using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] GameObject winGameUI;

    void Start()
    {
        FindObjectOfType<LevelManager>().OnLevelEnd += ShowWinGameUI;
    }

    private void ShowWinGameUI()
    {
        winGameUI.SetActive(true);
    }
}
