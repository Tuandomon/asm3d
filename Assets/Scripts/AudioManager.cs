using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tao bien luu tru audio source
public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    // tao bien luu tru audio Clip
    public AudioClip musicClip;
    public AudioClip winClip;

    private void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }
}


