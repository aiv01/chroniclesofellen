using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DoorAudio : MonoBehaviour
{
    private AudioSource source;
    [SerializeField]
    private AudioClip doorClip;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        source.PlayOneShot(doorClip);
    }
    public void StopAudio()
    {
        source.Stop();
        
    }


}
