using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject NPCPanel; //tham chieu den panel
    public TextMeshProUGUI NPCTextContent; //tham chieu den text
    public string[] content;

    Coroutine coroutine;

    private void Start()
    {
        NPCPanel.SetActive(false);
        NPCTextContent.text = "";
    }

    IEnumerator ReadContent()
    {
        NPCTextContent.text = "";
        foreach (var line in content)
        {
            for (int i = 0; i < line.Length; i++)
            {
                NPCTextContent.text += line[i];
                yield return new WaitForSeconds(0.2);
            }
            yield return new WaitForSeconds(2);
        }    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NPCPanel.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NPCPanel.SetActive(true);
            coroutine = StartCoroutine(ReadContent());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NPCPanel.SetActive(true);
            StopCoroutine(coroutine);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NPCPanel.SetActive(true);
        }
    }
} 
