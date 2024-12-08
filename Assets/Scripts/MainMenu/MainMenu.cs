using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu; // GameObject cho cửa sổ tùy chọn
    public GameObject loadingScreen; // Cửa sổ loading screen
    public Slider loadingBar; // Thanh tiến trình loading

    public void StartGame()
    {
        StartCoroutine(LoadGameAsync("Scene9")); // Thay "Scene9" bằng tên scene của game
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true); // Hiển thị cửa sổ tùy chọn
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false); // Đóng cửa sổ tùy chọn
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadGameAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }
    }
}

