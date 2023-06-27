using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] int level = 1;
    [SerializeField] WeaponSO weaponSO;
    [SerializeField] Sprite[] skillImages;

    protected int activeSkill = 0;

    protected virtual void Awake()
    {
        level = SaveLoadManager.WeaponLevel(weaponSO.weaponName);
    }

    void Start()
    {
        for(int i = 0; i < level; i++)
        {
            int skillIndex = i;
            InGameUI.Instance.AddSkill(i, () => ChangeSkill(skillIndex), skillImages[i]);
        }
    }

    void Update()
    {
        switch(activeSkill)
        {
            case 0: SkillOne();
                break;
            case 1: SkillTwo();
                break;
            case 2: SkillThree();
                break;
            case 3: SkillFour();
                break;
            case 4: SkillFive();
                break;
        }
    }

    protected virtual void ChangeSkill(int skill)
    {
        activeSkill = skill;
    }

    public abstract void SkillOne();
    public abstract void SkillTwo();
    public abstract void SkillThree();
    public abstract void SkillFour();
    public abstract void SkillFive();

}
