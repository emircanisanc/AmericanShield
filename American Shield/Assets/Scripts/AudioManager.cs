using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip buttonClip;

    public void PlayButtonClip()
    {
        PlayClipNearCamera(buttonClip);
    }

    public void PlayClipNearCamera(AudioClip audioClip)
    {
        if (audioClip)
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
    }

    public static void PlayClipAtCamera(AudioClip audioClip)
    {
        if (audioClip)
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
    }

    public static void PlayClip(AudioClip audioClip, Vector3 pos)
    {
        if (audioClip)
            AudioSource.PlayClipAtPoint(audioClip, pos);
    }

}
