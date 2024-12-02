using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Scene9"); // Thay "GameScene" bằng tên scene của game
    }

    public void OpenOptions()
    {
        // Mở cửa sổ tùy chọn hoặc scene tùy chọn
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
