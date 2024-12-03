using UnityEngine;
using UnityEngine.UI;

public class IncreaseHealth : MonoBehaviour
{
    public int health = 100; // Máu ban đầu của nhân vật
    public Button increaseHealthButton; // Nút để tăng máu

    void Start()
    {
        // Đảm bảo rằng nút được gắn đúng sự kiện
        increaseHealthButton.onClick.AddListener(IncreaseHealthPoints);
    }

    void IncreaseHealthPoints()
    {
        health += 10; // Tăng máu lên 10 điểm mỗi lần bấm nút
        Debug.Log("Health increased. Current health: " + health);
    }
}
