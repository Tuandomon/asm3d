using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : Health
{
    public Image healthBar; // Thay Slider bằng Image
    public TextMeshProUGUI healthText; // TextMeshProUGUI

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthBar.fillAmount = currentHP / maxHP;
        healthText.text = $"{currentHP:F0} / {maxHP:F0}"; // Hiển thị số máu
    }
}


