using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MP : MonoBehaviour
{
    public int MaxMP;
    public int CurrentMP;
    public float drainRate = 1f; // Số giây giảm 1 MP khi giữ shift
    public Image manaBar; // Thanh mana
    public TMP_Text manaText; // Văn bản hiển thị mana
    private float nextDrainTime = 0f;
    private bool isRecovering = false;
    public float damage; // Thêm thuộc tính damage nếu cần

    // Start is called before the first frame update
    void Start()
    {
        CurrentMP = MaxMP;
        UpdateManaUI(); // Cập nhật UI ngay từ đầu
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Time.time >= nextDrainTime && CurrentMP > 0)
            {
                CurrentMP--;
                nextDrainTime = Time.time + drainRate;
                UpdateManaUI();
            }
            isRecovering = false;
        }
        else
        {
            if (!isRecovering)
            {
                isRecovering = true;
                StartCoroutine(RecoverMP());
            }
        }
    }

    IEnumerator RecoverMP()
    {
        while (isRecovering && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            yield return new WaitForSeconds(2f); // Hồi 2 mana mỗi giây
            if (CurrentMP < MaxMP)
            {
                CurrentMP += 1;
                UpdateManaUI();
            }
        }
    }

    // Hàm cập nhật UI thanh mana
    public void UpdateManaUI()
    {
        if (manaBar != null)
        {
            manaBar.fillAmount = (float)CurrentMP / MaxMP;
        }
        if (manaText != null)
        {
            manaText.text = $"{CurrentMP} / {MaxMP}";
        }
    }
}

