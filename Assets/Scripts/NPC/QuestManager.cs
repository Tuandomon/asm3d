using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public GameObject questPanel; // Panel nhiệm vụ
    public TextMeshProUGUI questNameText; // Tên nhiệm vụ
    public TextMeshProUGUI killCountText; // Số lượng cần giết
    public TextMeshProUGUI killedCountText; // Số lượng đã giết
    public Button completeQuestButton; // Nút hoàn thành nhiệm vụ
    public Button cancelQuestButton; // Nút hủy nhiệm vụ

    public string questName = "Kill 10 Skeletons"; // Tên nhiệm vụ
    public string targetEnemyName = "Skeleton Lv1"; // Tên kẻ thù cần giết
    public int requiredKills = 10; // Số lượng cần giết
    public int currentKills = 0; // Số lượng đã giết
    public int expReward = 100; // Kinh nghiệm nhận được sau khi hoàn thành nhiệm vụ

    private PlayerExperience playerExperience;

    private void Start()
    {
        questPanel.SetActive(false); // Ẩn bảng nhiệm vụ ban đầu
        completeQuestButton.onClick.AddListener(CompleteQuest);
        cancelQuestButton.onClick.AddListener(CancelQuest);

        // Lấy tham chiếu đến PlayerExperience
        playerExperience = FindObjectOfType<PlayerExperience>();
    }

    private void UpdateQuestUI()
    {
        questNameText.text = questName;
        killCountText.text = $"Kills Needed: {requiredKills}";
        killedCountText.text = $"Kills Done: {currentKills}";
        completeQuestButton.interactable = currentKills >= requiredKills; // Kích hoạt nút hoàn thành nếu đủ số lượng giết
    }

    public void ShowQuestPanel()
    {
        questPanel.SetActive(true);
        UpdateQuestUI();
    }

    public void UpdateKillCount(string enemyName)
    {
        if (enemyName == targetEnemyName)
        {
            currentKills++;
            UpdateQuestUI();
        }
    }

    private void CompleteQuest()
    {
        if (currentKills >= requiredKills && playerExperience != null)
        {
            playerExperience.AddExperience(expReward);
            questPanel.SetActive(false); // Ẩn bảng nhiệm vụ sau khi hoàn thành
            ResetQuest(); // Reset nhiệm vụ sau khi hoàn thành
        }
    }

    private void CancelQuest()
    {
        questPanel.SetActive(false); // Ẩn bảng nhiệm vụ khi hủy
        ResetQuest(); // Reset nhiệm vụ khi hủy
    }

    private void ResetQuest()
    {
        currentKills = 0; // Đặt lại số lượng đã giết về 0
        UpdateQuestUI();
    }
}
