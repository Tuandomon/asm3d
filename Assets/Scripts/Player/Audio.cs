using UnityEngine;

public class PlayerHitByEnemy : MonoBehaviour
{
    public AudioSource audioSource;  // Thành phần AudioSource để phát âm thanh
    public AudioClip hitSound;       // File âm thanh khi bị tấn công
    public int health = 100;         // Máu của nhân vật
    public int damagePerHit = 10;    // Số máu mất mỗi lần bị tấn công
    public float hitCooldown = 1.0f; // Thời gian hồi để tránh tấn công liên tục
    private float lastHitTime;       // Lưu thời gian lần tấn công cuối cùng

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();  // Lấy AudioSource trên nhân vật
        }
        lastHitTime = -hitCooldown;  // Đảm bảo có thể bị tấn công ngay khi bắt đầu
    }

    // Hàm xử lý khi bị tấn công
    public void TakeDamage()
    {
        // Kiểm tra cooldown để tránh bị tấn công quá nhanh
        if (Time.time - lastHitTime >= hitCooldown)
        {
            health -= damagePerHit;

            // Phát âm thanh nếu còn máu
            if (health > 0 && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);  // Phát âm thanh bị tấn công
            }

            // Nếu máu = 0, xử lý nhân vật chết
            if (health <= 0)
            {
                Debug.Log("Nhân vật đã chết!");
                // Thêm logic chết tại đây (ví dụ: dừng game, hiện màn hình Game Over, v.v.)
            }

            // Cập nhật thời gian bị tấn công
            lastHitTime = Time.time;
        }
    }

    // Xử lý khi va chạm với kẻ địch
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))  // Kiểm tra tag của đối tượng va chạm
        {
            TakeDamage();
            Debug.Log("Nhân vật bị tấn công bởi Enemy!");
        }
    }

    // Hoặc nếu kẻ địch tấn công qua trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage();
            Debug.Log("Nhân vật bị tấn công bởi Enemy!");
        }
    }
}
