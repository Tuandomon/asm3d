using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class Scripts_
{
    private GameObject player;
    private Character character;
    private PlayerHealth playerHealth;

    [SetUp]
    public void SetUp()
    {
        // Tạo đối tượng Player và thêm các thành phần cần thiết
        player = new GameObject("Player");
        player.AddComponent<CharacterController>(); // Thêm CharacterController để xử lý di chuyển
        character = player.AddComponent<Character>(); // Gắn script Character

        // Thiết lập các giá trị mặc định cho script Character
        character.characterController = player.GetComponent<CharacterController>();

        // Sử dụng AddComponent thay cho new để tránh lỗi với MonoBehaviour
        var inputObject = new GameObject("PlayerInput");
        character.playerInput = inputObject.AddComponent<PlayerInput>();
        character.playerInput.horizontalInput = 0;
        character.playerInput.verticalInput = 0;
        character.playerInput.attackInput = false;

        // Gắn Animator và thêm Animator Controller giả lập (nếu cần)
        character.animator = player.AddComponent<Animator>();
        RuntimeAnimatorController animatorController = Resources.Load<RuntimeAnimatorController>("Path/To/AnimatorController");
        if (animatorController != null)
        {
            character.animator.runtimeAnimatorController = animatorController;
        }

        // Gắn Health và khởi tạo giá trị
        character.health = new Health { currentHP = 100 };

        // Đặt trạng thái mặc định
        character.currentState = Character.CharacterState.Normal;

        // Tạo PlayerHealth và thiết lập cho kiểm thử UI
        playerHealth = player.AddComponent<PlayerHealth>();

        // Tạo UI giả lập cho thanh máu
        var healthBarObject = new GameObject("HealthBar");
        healthBarObject.AddComponent<Canvas>();
        playerHealth.healthBar = healthBarObject.AddComponent<Image>();
        playerHealth.healthBar.fillAmount = 1f; // Ban đầu đầy máu

        var healthTextObject = new GameObject("HealthText");
        healthTextObject.AddComponent<Canvas>();
        playerHealth.healthText = healthTextObject.AddComponent<TextMeshProUGUI>();
        playerHealth.maxHP = 100;
        playerHealth.currentHP = 90; // Khởi tạo máu thấp hơn maxHP để kiểm tra hồi máu
        playerHealth.regenRate = 6f; // Thiết lập thời gian hồi máu 6 giây
        playerHealth.UpdateHealthUI(); // Cập nhật giao diện UI
    }

    [UnityTest]
    public IEnumerator CharacterMovesForwardWithInput()
    {
        // Arrange: Đặt giá trị đầu vào di chuyển về phía trước
        character.playerInput.verticalInput = 1; // Mô phỏng nhấn phím "W"
        Vector3 initialPosition = player.transform.position;

        // Act: Gọi SimulateFixedUpdate để kiểm tra logic di chuyển
        character.SimulateFixedUpdate(); // Gọi phương thức công khai thay vì FixedUpdate
        yield return null; // Đợi một khung hình để kiểm tra kết quả

        // Assert: Kiểm tra vị trí của nhân vật đã thay đổi
        Vector3 newPosition = player.transform.position;
        Assert.Greater(newPosition.z, initialPosition.z, "Nhân vật không di chuyển về phía trước với đầu vào.");
    }

    [UnityTest]
    public IEnumerator CharacterChangesStateToAttackBasedOnInput()
    {
        // Arrange: Đặt giá trị đầu vào tấn công
        character.playerInput.attackInput = true;

        // Act: Gọi SimulateFixedUpdate để kiểm tra logic thay đổi trạng thái
        character.SimulateFixedUpdate();
        yield return null; // Đợi một khung hình để kiểm tra kết quả

        // Assert: Kiểm tra trạng thái đã thay đổi thành "Attack"
        Assert.AreEqual(Character.CharacterState.Attack, character.currentState, "Nhân vật không chuyển sang trạng thái Attack khi nhận đầu vào tấn công.");
    }

    [UnityTest]
    public IEnumerator PlayerHealthRegenerationUpdatesUI()
    {
        // Arrange: Xác minh thanh máu ban đầu
        Assert.AreEqual(0.9f, playerHealth.healthBar.fillAmount, "Thanh máu khởi tạo không đúng.");
        Assert.AreEqual("90 / 100", playerHealth.healthText.text, "Text máu khởi tạo không đúng.");

        // Act: Chờ vượt qua thời gian hồi máu
        yield return new WaitForSeconds(6f); // Chờ chính xác thời gian hồi máu
        playerHealth.ForceHealthRegeneration(); // Cập nhật trực tiếp máu trong kiểm thử

        // Assert: Kiểm tra UI đã cập nhật sau hồi máu
        Assert.AreEqual(91, playerHealth.currentHP, "Máu không hồi đúng sau 6 giây.");
        Assert.AreEqual(0.91f, playerHealth.healthBar.fillAmount, "Thanh máu không cập nhật đúng.");
        Assert.AreEqual("91 / 100", playerHealth.healthText.text, "Text máu không cập nhật đúng.");
    }
    [TearDown]
    public void TearDown()
    {
        // Dọn dẹp các đối tượng đã tạo sau mỗi bài kiểm thử
#if UNITY_EDITOR
        Object.DestroyImmediate(player); // Trong Edit Mode
#else
        Object.Destroy(player); // Trong Play Mode
#endif
    }
}