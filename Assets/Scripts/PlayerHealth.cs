using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : Health
{
    public Image healthBar; // Thay Slider bằng Image
    public TextMeshProUGUI healthText; // TextMeshProUGUI
    private float lastDamageTime; // Thời gian lần cuối nhận damage
    private bool isRecovering;

    public float regenRate = 6f; // Mặc định là 6 giây hồi 1 máu

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
        isRecovering = false;
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
        SimulateUpdate(); // Gọi logic qua phương thức công khai
    }

    // Phương thức công khai để kiểm tra logic của Update
    public void SimulateUpdate()
    {
        if (!isRecovering && Time.time >= lastDamageTime + 2f)
        {
            Debug.Log("Starting health regeneration...");
            isRecovering = true;
            StartCoroutine(RecoverHealth());
        }
    }

    private IEnumerator RecoverHealth()
    {
        while (isRecovering)
        {
            Debug.Log($"Recovering... Current HP: {currentHP}, Max HP: {maxHP}");
            yield return new WaitForSeconds(regenRate); // Chờ theo thời gian regenRate
            if (currentHP < maxHP)
            {
                currentHP = Mathf.Min(currentHP + 1, maxHP); // Hồi 1 máu
                UpdateHealthUI();
                Debug.Log($"Recovered to {currentHP} HP.");
            }
            else
            {
                Debug.Log("Stopping recovery: HP is full.");
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

    public void ForceHealthRegeneration()
    {
        if (currentHP < maxHP)
        {
            currentHP = Mathf.Min(currentHP + 1, maxHP); // Hồi 1 máu
            UpdateHealthUI();
        }
    }
}