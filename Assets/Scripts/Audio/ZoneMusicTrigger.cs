using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneMusicTrigger : MonoBehaviour
{
    public AudioClip zoneMusicClip; // Bản nhạc nền cho vùng này
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioManager != null)
            {
                audioManager.AddZoneMusic(zoneMusicClip);
                audioManager.SetPlayerInZone(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioManager != null)
            {
                audioManager.SetPlayerInZone(false);
            }
        }
    }
}



