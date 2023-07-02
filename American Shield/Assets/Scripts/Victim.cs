using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    [SerializeField] GameObject[] toggleObjects;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var obj in toggleObjects)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }    
    }
}
