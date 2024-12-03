using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public int npcHealth = 100;
    public float npcLifetime = 30f;

    // Hàm để tạo NPC
    void SpawnNPC()
    {
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        // Thêm script điều khiển cho NPC
        NPCController npcController = npc.GetComponent<NPCController>();
        if (npcController != null)
        {
            npcController.Initialize(npcHealth, npcLifetime);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SpawnNPC();
        }
    }
}

public class NPCController : MonoBehaviour
{
    public int health;
    public float lifetime;
    private float spawnTime;

    public void Initialize(int initialHealth, float lifetime)
    {
        this.health = initialHealth;
        this.lifetime = lifetime;
        this.spawnTime = Time.time;
    }

    void Update()
    {
        // Kiểm tra thời gian sống
        if (Time.time - spawnTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
