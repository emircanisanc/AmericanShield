using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image imageSkill;

    public void SetSkill(UnityAction action, Sprite sprite)
    {
        imageSkill.sprite = sprite;
        button.onClick.AddListener(action);
    }
}
