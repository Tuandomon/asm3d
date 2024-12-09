using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DragonHealth : Health
{
    public Image healthBar; // Thay Slider bằng Image
    public TextMeshProUGUI healthText; // TextMeshProUGUI
    public GameObject healthCanvas; // Canvas máu của Dragon
    private float lastDamageTime; // Thời gian lần cuối nhận damage
    private bool isRecovering;
    public Transform player; // Transform của người chơi
    public float displayRange = 10f; // Phạm vi hiển thị Canvas máu

    public float regenRate = 6f; // Mặc định là 6 giây hồi 1 máu

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
        isRecovering = false;
        healthCanvas.SetActive(false); // Ẩn Canvas máu khi bắt đầu
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UpdateHealthUI();
        lastDamageTime = Time.time; // Cập nhật thời gian nhận damage cuối cùng
        isRecovering = false; // Ngừng hồi máu khi nhận damage
    }

    private void Update()
    {
        if (!isRecovering && Time.time >= lastDamageTime + 2f)
        {
            isRecovering = true;
            StartCoroutine(RecoverHealth());
        }

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= displayRange)
        {
            healthCanvas.SetActive(true);
        }
        else
        {
            healthCanvas.SetActive(false);
        }
    }

    private IEnumerator RecoverHealth()
    {
        while (isRecovering)
        {
            yield return new WaitForSeconds(regenRate); // Đợi theo thời gian regenRate
            if (currentHP < maxHP)
            {
                currentHP = Mathf.Min(currentHP + 1, maxHP); // Hồi 1 máu
                UpdateHealthUI();
            }
            else
            {
                isRecovering = false;
            }
        }
    }

    public void UpdateHealthUI()
    {
        healthBar.fillAmount = (float)currentHP / maxHP;
        healthText.text = $"{currentHP:F0} / {maxHP:F0}"; // Hiển thị số máu
    }

    public void IncreaseMaxHP(int amount)
    {
        maxHP += amount;
        UpdateHealthUI();
    }
}





