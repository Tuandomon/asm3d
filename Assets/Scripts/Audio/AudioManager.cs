using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource; // AudioSource để phát nhạc nền
    public AudioSource vfxAudioSource;   // AudioSource để phát hiệu ứng âm thanh (VFX)

    // Tạo biến lưu trữ danh sách các audio clip cho nhạc nền
    public List<AudioClip> musicClips;
    public AudioClip winClip;

    // Biến để lưu trữ khoảng cách phát nhạc
    public float delayBetweenTracks = 5f; // Khoảng cách phát nhạc (giây)

    private bool isPlayingZoneMusic = false; // Biến để kiểm tra trạng thái phát nhạc vùng
    private bool playerInZone = false; // Biến để kiểm tra nếu nhân vật còn trong vùng nhạc
    private Queue<AudioClip> zoneMusicQueue = new Queue<AudioClip>(); // Hàng đợi nhạc vùng
    private AudioClip currentZoneClip = null; // Lưu trữ clip vùng hiện tại

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
            if (!isPlayingZoneMusic)
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
            else
            {
                if (zoneMusicQueue.Count > 0)
                {
                    currentZoneClip = zoneMusicQueue.Dequeue();
                    musicAudioSource.clip = currentZoneClip;
                    musicAudioSource.Play();
                    yield return new WaitForSeconds(musicAudioSource.clip.length);
                }
                else
                {
                    isPlayingZoneMusic = false;
                    currentZoneClip = null;
                    if (playerInZone)
                    {
                        // Reset queue and continue playing zone music if player re-enters
                        StartCoroutine(PlayZoneMusic());
                    }
                }
            }
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

    public void AddZoneMusic(AudioClip clip)
    {
        if (currentZoneClip != clip)
        {
            StopAllCoroutines();
            zoneMusicQueue.Clear();
            isPlayingZoneMusic = true;
            zoneMusicQueue.Enqueue(clip);
            StartCoroutine(PlayZoneMusic());
        }
    }

    private IEnumerator PlayZoneMusic()
    {
        while (zoneMusicQueue.Count > 0)
        {
            currentZoneClip = zoneMusicQueue.Dequeue();
            musicAudioSource.clip = currentZoneClip;
            musicAudioSource.Play();
            yield return new WaitForSeconds(musicAudioSource.clip.length);
        }
        isPlayingZoneMusic = false;
        currentZoneClip = null;
        StartCoroutine(PlayMusic());
    }

    public void SetPlayerInZone(bool inZone)
    {
        playerInZone = inZone;
        if (!inZone)
        {
            StopAllCoroutines();
            isPlayingZoneMusic = false;
            currentZoneClip = null;
            StartCoroutine(PlayMusic());
        }
    }
}






