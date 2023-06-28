using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager
{
    public const string DEFAULTWEAPONNAME = "InfinityGlow";
    public const int DEFAULTWEAPONLEVEL = 2;
    public const int GAMEDEFAULTLEVEL = 2;

    public static string CurrentWeaponName()
    {
        return PlayerPrefs.GetString("currentWeapon", DEFAULTWEAPONNAME);
    }

    public static void SaveCurrentWeapon(string weaponName)
    {
        PlayerPrefs.SetString("currentWeapon", weaponName);
    }
    
    public static int WeaponLevel(string weaponName)
    {
        return PlayerPrefs.GetInt(weaponName + "level", DEFAULTWEAPONLEVEL);
    }

    public static bool WeaponIsUnclocked(string weaponName)
    {
        return PlayerPrefs.GetInt(weaponName+"isUnlocked", 1) == 1;
    }

    public static int WeaponExp(string weaponName)
    {
        return PlayerPrefs.GetInt(weaponName + "exp", 0);
    }

    public static void SaveWeaponLevel(string weaponName, int level)
    {
        PlayerPrefs.SetInt(weaponName + "level", level);
    }

    public static void SaveWeaponExp(string weaponName, int exp)
    {
        PlayerPrefs.SetInt(weaponName + "exp", exp);
    }
    
    public static void SetLevel(int level)
    {
        PlayerPrefs.SetInt("gameLevel", level);
    }

    public static int CurrentLevel()
    {
        return PlayerPrefs.GetInt("gameLevel", GAMEDEFAULTLEVEL);
    }
}
