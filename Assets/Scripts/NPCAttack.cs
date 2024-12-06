using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // Phạm vi tấn công
    public int damage = 10; // Sát thương gây ra
    public float detectionRadius = 10f; // Bán kính tìm mục tiêu
    public float followDistance = 2f; // Khoảng cách giữ với Player
    public float behindDistance = 1f; // Khoảng cách đứng phía sau Player
    public float attackCooldown = 2f; // Thời gian hồi chiêu tấn công (giây)
    public float moveSpeed = 3.5f; // Tốc độ di chuyển của NPC
    public float lifetime = 10f; // Thời gian tồn tại của NPC (giây)

    private float lastAttackTime = 0f; // Thời gian tấn công cuối cùng
    private GameObject[] enemies;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Transform player;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed; // Thiết lập tốc độ di chuyển của NavMeshAgent
        animator = GetComponent<Animator>();

        // Bắt đầu đếm ngược thời gian tồn tại của NPC
        StartCoroutine(NPCDespawnCountdown());
    }

    void Update()
    {
        if (player == null) return;

        // Tìm tất cả các đối tượng có tag "Enemy" trong bán kính detectionRadius
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = detectionRadius;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null && closestDistance <= detectionRadius)
        {
            // Di chuyển tới kẻ thù gần nhất nếu có
            navMeshAgent.SetDestination(closestEnemy.transform.position);

            // Tính khoảng cách giữa NPC và kẻ thù
            if (closestDistance <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack(closestEnemy);
                    lastAttackTime = Time.time;
                }
            }
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
        else
        {
            // Nếu không có kẻ thù trong bán kính, di chuyển theo Player
            Vector3 followPosition = player.position - player.forward * behindDistance;
            if (Vector3.Distance(transform.position, player.position) > followDistance)
            {
                navMeshAgent.SetDestination(followPosition);
            }
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
    }

    void Attack(GameObject enemy)
    {
        // Ví dụ: Giảm máu của kẻ thù bằng cách gọi hàm TakeDamage
        enemy.GetComponent<Health>().TakeDamage(damage);
        animator.SetTrigger("Attack");
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    private IEnumerator NPCDespawnCountdown()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject); // Hủy đối tượng NPC sau thời gian tồn tại
    }
}



