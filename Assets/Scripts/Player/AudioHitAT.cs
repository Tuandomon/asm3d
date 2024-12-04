using UnityEngine;

public class PlaySoundOnClick : MonoBehaviour
{
    public AudioSource audioSource;  // Thành phần AudioSource để phát âm thanh
    public AudioClip clickSound;    // File âm thanh khi nhấn chuột

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); // Tìm AudioSource trong GameObject
        }
    }

    void Update()
    {
        // Kiểm tra nếu chuột trái được nhấn
        if (Input.GetMouseButtonDown(0)) // 0: nút chuột trái
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound); // Phát âm thanh
        }
    }
}
