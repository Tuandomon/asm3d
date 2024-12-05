using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    // Tạo biến lưu trữ danh sách các audio clip cho nhạc nền
    public List<AudioClip> musicClips;
    public AudioClip winClip;

    // Biến để lưu trữ khoảng cách phát nhạc
    public float delayBetweenTracks = 5f; // Khoảng cách phát nhạc (giây)

    private void Start()
    {
        if (musicClips.Count > 0)
        {
            StartCoroutine(PlayMusic());
        }
        else
        {
            Debug.LogWarning("No music clips assigned to AudioManager!");
        }
    }

    private IEnumerator PlayMusic()
    {
        while (true)
        {
            if (musicClips.Count > 1)
            {
                // Phát ngẫu nhiên một bản nhạc từ danh sách
                int randomIndex = Random.Range(0, musicClips.Count);
                musicAudioSource.clip = musicClips[randomIndex];
            }
            else if (musicClips.Count == 1)
            {
                // Phát bản nhạc duy nhất trong danh sách
                musicAudioSource.clip = musicClips[0];
            }
            else
            {
                // Không có bản nhạc nào để phát, thoát khỏi vòng lặp
                yield break;
            }

            musicAudioSource.Play();
            yield return new WaitForSeconds(musicAudioSource.clip.length + delayBetweenTracks);
        }
    }

    public void PlayWinMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = winClip;
        musicAudioSource.Play();
    }

    public void PlayVFX(AudioClip clip)
    {
        vfxAudioSource.PlayOneShot(clip);
    }
}



