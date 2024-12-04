using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // Phạm vi tấn công
    public int damage = 10; // Sát thương gây ra
    private GameObject[] enemies;

    void Update()
    {
        // Tìm tất cả các đối tượng có tag "Enemy"
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Tính khoảng cách giữa NPC và kẻ thù
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Nếu kẻ thù nằm trong phạm vi tấn công, tiến hành tấn công
            if (distance <= attackRange)
            {
                Attack(enemy);
            }
        }
    }

    void Attack(GameObject enemy)
    {
        // Ví dụ: Giảm máu của kẻ thù bằng cách gọi hàm TakeDamage
        enemy.GetComponent<Health>().TakeDamage(damage);
    }
}
