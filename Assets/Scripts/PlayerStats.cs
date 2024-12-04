using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public GameObject statsPanel; // Panel chứa bảng thống kê
    public Button healthButton;
    public Button mpButton;
    public Button healingButton;
    public Button strengthButton;

    private PlayerHealth playerHealth;
    private MP playerMP;
    private Character character;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMP = GetComponent<MP>();
        character = GetComponent<Character>();

        statsPanel.SetActive(false);

        healthButton.onClick.AddListener(IncreaseHealth);
        mpButton.onClick.AddListener(IncreaseMP);
        healingButton.onClick.AddListener(DecreaseRegenRate);
        strengthButton.onClick.AddListener(IncreaseStrength);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleStatsPanel();
        }
    }

    private void ToggleStatsPanel()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    private void IncreaseHealth()
    {
        playerHealth.maxHP += 2; // Tăng maxHP thêm 2
        playerHealth.UpdateHealthUI();
    }

    private void IncreaseMP()
    {
        playerMP.IncreaseMaxMP(2); // Tăng MaxMP thêm 2
        playerMP.UpdateManaUI();
    }

    private void DecreaseRegenRate()
    {
        playerHealth.regenRate = Mathf.Max(1f, playerHealth.regenRate - 0.2f); // Giảm thời gian hồi máu đi 0,2 giây nhưng không dưới 1 giây
    }

    private void IncreaseStrength()
    {
        character.IncreaseCharacterDamage(2); // Tăng damage của Character thêm 2
    }
}



