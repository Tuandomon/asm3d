using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Thư viện TextMeshPro
using UnityEngine.UI;

public class DragonNPC : MonoBehaviour
{
    public GameObject NPCPanel; // Tham chiếu đến panel
    public TextMeshProUGUI NPCTextContent; // Tham chiếu đến text
    public string[] content; // Nội dung NPC

    public Button ContinueButton; // Tham chiếu đến nút Tiếp tục
    public Button CancelButton; // Tham chiếu đến nút Hủy

    public CinemachineFreeLook freeLookCamera; // Tham chiếu đến Cinemachine FreeLook camera
    public PlayerInput playerInput; // Tham chiếu đến script đầu vào của người chơi

    private Coroutine coroutine;
    private int currentLineIndex = 0;
    private bool isReading = false;
    private bool hasTalked = false; // Biến kiểm tra đã trò chuyện hay chưa

    private void Start()
    {
        NPCPanel.SetActive(false);
        NPCTextContent.text = "";

        // Gán sự kiện cho nút
        ContinueButton.onClick.AddListener(ContinueDialogue);
        CancelButton.onClick.AddListener(CancelDialogue);
    }
    
    IEnumerator ReadContent()
    {
        NPCTextContent.text = ""; // Xóa nội dung cũ
        foreach (var line in content[currentLineIndex])
        {
            NPCTextContent.text += line;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f); // Dừng lại sau mỗi câu thoại
        isReading = false; // Cho phép tiếp tục câu thoại khác
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasTalked)
        {
            NPCPanel.SetActive(true);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            freeLookCamera.enabled = false; // Vô hiệu hóa camera xoay hướng
            playerInput.enabled = false; // Vô hiệu hóa đầu vào của người chơi
            StartDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NPCPanel.SetActive(false);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            freeLookCamera.enabled = true; // Bật lại camera xoay hướng
            playerInput.enabled = true; // Bật lại đầu vào của người chơi
            ResetDialogue();
        }
    }

    private void StartDialogue()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        isReading = true;
        coroutine = StartCoroutine(ReadContent());
    }

    private void ContinueDialogue()
    {
        if (!isReading && currentLineIndex < content.Length - 1)
        {
            currentLineIndex++;
            isReading = true;
            coroutine = StartCoroutine(ReadContent());
        }
        else if (!isReading && currentLineIndex == content.Length - 1)
        {
            hasTalked = true; // Đánh dấu đã trò chuyện xong
            CancelDialogue();
        }
    }

    private void CancelDialogue()
    {
        NPCPanel.SetActive(false);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        freeLookCamera.enabled = true; // Bật lại camera xoay hướng
        playerInput.enabled = true; // Bật lại đầu vào của người chơi
        ResetDialogue();
    }

    private void ResetDialogue()
    {
        currentLineIndex = 0;
        NPCTextContent.text = "";
        isReading = false;
    }
}

