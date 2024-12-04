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
    public Button cancelButton; // Nút Cancel
    public TMP_Text statPointsText; // Văn bản hiển thị điểm có thể nâng
    public Button upgradeButton; // Nút hiển thị khi có điểm nâng cấp

    private PlayerHealth playerHealth;
    private MP playerMP;
    private Character character;
    private PlayerExperience playerExperience;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMP = GetComponent<MP>();
        character = GetComponent<Character>();
        playerExperience = GetComponent<PlayerExperience>();

        statsPanel.SetActive(false);

        healthButton.onClick.AddListener(IncreaseHealth);
        mpButton.onClick.AddListener(IncreaseMP);
        healingButton.onClick.AddListener(DecreaseRegenRate);
        strengthButton.onClick.AddListener(IncreaseStrength);
        cancelButton.onClick.AddListener(HideStatsPanel); // Gán sự kiện cho nút Cancel
        upgradeButton.onClick.AddListener(ShowStatsPanel); // Gán sự kiện cho nút Upgrade

        UpdateStatPointsUI();
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
        UpdateStatPointsUI();
    }

    private void HideStatsPanel()
    {
        statsPanel.SetActive(false);
    }

    private void ShowStatsPanel()
    {
        statsPanel.SetActive(true);
        UpdateStatPointsUI();
    }

    private void IncreaseHealth()
    {
        if (playerExperience.SpendStatPoint())
        {
            playerHealth.maxHP += 2; // Tăng maxHP thêm 2
            playerHealth.UpdateHealthUI();
        }
    }

    private void IncreaseMP()
    {
        if (playerExperience.SpendStatPoint())
        {
            playerMP.IncreaseMaxMP(2); // Tăng MaxMP thêm 2
            playerMP.UpdateManaUI();
        }
    }

    private void DecreaseRegenRate()
    {
        if (playerExperience.SpendStatPoint())
        {
            playerHealth.regenRate = Mathf.Max(1f, playerHealth.regenRate - 0.2f); // Giảm thời gian hồi máu đi 0,2 giây nhưng không dưới 1 giây
        }
    }

    private void IncreaseStrength()
    {
        if (playerExperience.SpendStatPoint())
        {
            character.IncreaseCharacterDamage(2); // Tăng damage của Character thêm 2
        }
    }

    private void UpdateStatPointsUI()
    {
        statPointsText.text = $"Stat Points: {playerExperience.statPoints}"; // Hiển thị điểm có thể nâng
    }
}





