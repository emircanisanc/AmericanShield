using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStoneSpawner : MonoBehaviour
{
    public GameObject[] stones;

    public void LoadStones(int level)
    {
        for (int i = 0; i < level; i++)
        {
            stones[i].SetActive(true);
        }
    }
}
