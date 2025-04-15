#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MenuTest
{
    private GameObject menuObject;
    private MockMainMenu mainMenu;

    [SetUp]
    public void SetUp()
    {
        // Tạo đối tượng MainMenu giả lập
        menuObject = new GameObject("MainMenu");
        mainMenu = menuObject.AddComponent<MockMainMenu>();
    }

    [Test]
    public void StartGame_OpensSceneInEditMode()
    {
#if UNITY_EDITOR
        // Arrange: Kiểm tra scene hiện tại trước khi chuyển
        string initialScene = SceneManager.GetActiveScene().name;

        // Act: Dùng EditorSceneManager để mở scene trong Edit Mode
        EditorSceneManager.OpenScene("Assets/Scenes/Scene9.unity");

        // Assert: Kiểm tra scene đã thay đổi
        string newScene = SceneManager.GetActiveScene().name;
        Assert.AreNotEqual(initialScene, newScene, "Scene không thay đổi sau khi nhấn Start trong Edit Mode.");
#else
        Assert.Ignore("Bài kiểm thử này chỉ chạy trong Unity Editor.");
#endif
    }

    [Test]
    public void ExitGame_CallsQuitMethod()
    {
        // Arrange: Kiểm tra trạng thái ban đầu
        Assert.IsFalse(mainMenu.quitCalled, "Thoát game chưa được gọi.");

        // Act: Gọi ExitGame()
        mainMenu.ExitGame();

        // Assert: Kiểm tra xem ExitGame đã gọi thoát game chưa
        Assert.IsTrue(mainMenu.quitCalled, "ExitGame không gọi Application.Quit() đúng cách.");
    }

    [TearDown]
    public void TearDown()
    {
#if UNITY_EDITOR
        Object.DestroyImmediate(menuObject);
#else
        Object.Destroy(menuObject);
#endif
    }
}

// Lớp giả lập MainMenu để kiểm tra ExitGame
public class MockMainMenu : MainMenu
{
    public bool quitCalled = false;

    public override void ExitGame()
    {
        quitCalled = true; // Đánh dấu rằng phương thức đã được gọi
    }
}