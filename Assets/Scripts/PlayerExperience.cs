using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    public int currentExp;
    public int requiredExp;
    public int level;
    public Image expBar; // Thanh kinh nghiệm
    public TMP_Text expText; // Văn bản hiển thị kinh nghiệm
    public TMP_Text levelText; // Văn bản hiển thị cấp độ

    private PlayerHealth playerHealth;
    private MP playerMP;
    private DamageZone damageZone;

    private void Start()
    {
        level = 1;
        currentExp = 0;
        requiredExp = 50; // Yêu cầu kinh nghiệm ban đầu để lên cấp
        playerHealth = GetComponent<PlayerHealth>();
        playerMP = GetComponent<MP>();
        damageZone = GetComponentInChildren<DamageZone>();
        UpdateExpUI();
    }

    private void Update()
    {
        // Kiểm tra xem phím Space có được nhấn không để thêm kinh nghiệm
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddExperience(20); // Thêm 20 điểm kinh nghiệm mỗi lần nhấn Space
        }
    }

    private void UpdateExpUI()
    {
        expBar.fillAmount = (float)currentExp / requiredExp;
        expText.text = $"{currentExp} / {requiredExp}"; // Hiển thị số kinh nghiệm
        levelText.text = $"Lv {level}"; // Hiển thị cấp độ hiện tại
    }

    public void AddExperience(int amount)
    {
        currentExp += amount;
        while (currentExp >= requiredExp)
        {
            LevelUp();
        }
        UpdateExpUI();
    }

    private void LevelUp()
    {
        currentExp -= requiredExp; // Giữ lại phần dư kinh nghiệm cho cấp độ tiếp theo
        level++;
        requiredExp += level * 10; // Tăng yêu cầu kinh nghiệm tùy chỉnh cho cấp độ mới

        // Tăng máu, MP và damage theo cấp độ
        int healthIncrease = level; // Tăng máu theo cấp độ
        int manaIncrease = level; // Tăng MP theo cấp độ
        int damageIncrease = 2; // Tăng damage thêm 2

        playerHealth.maxHP += healthIncrease;
        playerHealth.UpdateHealthUI(); // Cập nhật UI sức khỏe

        playerMP.IncreaseMaxMP(manaIncrease);

        damageZone.IncreaseDamage(damageIncrease);

        UpdateExpUI();
    }

    /*
    Cấp 1: Yêu cầu 50 điểm kinh nghiệm để lên cấp.
    Cấp 2: Yêu cầu 50 + (2 * 10) = 70 điểm kinh nghiệm để lên cấp.
    Cấp 3: Yêu cầu 70 + (3 * 10) = 100 điểm kinh nghiệm để lên cấp.
    Cấp 4: Yêu cầu 100 + (4 * 10) = 140 điểm kinh nghiệm để lên cấp.
    
    Cấp độ (Level)	Kinh nghiệm yêu cầu (Required EXP)
        1	0
        2	50
        3	70
        4	100
        5	140
        6	190
        7	250
        8	320
        9	400
        10	490
        11	590
        12	700
        13	820
        14	950
        15	1090
        16	1240
        17	1400
        18	1570
        19	1750
        20	1940
    */
}
