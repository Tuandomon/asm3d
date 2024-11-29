using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    public GameObject NPCPanel; // Tham chiếu đến panel
    public TextMeshProUGUI NPCTextContent; // Tham chiếu đến text
    public string[] content; // Nội dung NPC

    public Button ContinueButton; // Tham chiếu đến nút Tiếp tục
    public Button CancelButton; // Tham chiếu đến nút Hủy

    private Coroutine coroutine;
    private int currentLineIndex = 0;
    private bool isReading = false;

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
        yield return new WaitForSeconds(2f); // Dừng lại sau mỗi câu thoại
        isReading = false; // Cho phép tiếp tục câu thoại khác
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NPCPanel.SetActive(true);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
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
    }

    private void CancelDialogue()
    {
        NPCPanel.SetActive(false);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        ResetDialogue();
    }

    private void ResetDialogue()
    {
        currentLineIndex = 0;
        NPCTextContent.text = "";
        isReading = false;
    }
}
