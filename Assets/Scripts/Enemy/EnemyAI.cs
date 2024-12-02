using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform target; // Mục tiêu

    public string enemyName; // Tên của kẻ thù
    public float radius = 10f; // Bán kính tìm kiếm mục tiêu
    public Vector3 originalePosition; // Vị trí ban đầu
    public float maxDistance = 50f; // Khoảng cách tối đa

    public Animator animator;

    public DamageZone damageZone;

    public Health health;

    public float attackCooldown = 2f; // Thời gian chờ giữa các lần tấn công
    private float lastAttackTime = -Mathf.Infinity; // Thời điểm lần tấn công cuối cùng

    public enum CharacterState
    {
        Normal,
        Attack,
        Die
    }
    public CharacterState currentState; // Trạng thái hiện tại
    private bool expAdded;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalePosition = transform.position;
    }

    void Update()
    {
        if (health.currentHP <= 0 && currentState != CharacterState.Die)
        {
            ChangeState(CharacterState.Die);
        }

        // Xoay hướng về mục tiêu
        if (target != null)
        {
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
        }

        if (currentState == CharacterState.Die)
        {
            return;
        }

        var distanceToOriginal = Vector3.Distance(originalePosition, target.position);
        var distance = Vector3.Distance(target.position, transform.position);

        if (distance <= radius && distanceToOriginal <= maxDistance)
        {
            navMeshAgent.SetDestination(target.position);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            distance = Vector3.Distance(target.position, transform.position);
            if (distance < 2f && Time.time >= lastAttackTime + attackCooldown)
            {
                ChangeState(CharacterState.Attack);
                lastAttackTime = Time.time;
            }
        }
        else if (distance > radius || distanceToOriginal > maxDistance)
        {
            navMeshAgent.SetDestination(originalePosition);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            distance = Vector3.Distance(originalePosition, transform.position);
            if (distance < 1f)
            {
                animator.SetFloat("Speed", 0);
            }

            ChangeState(CharacterState.Normal);
        }
    }
    private void ChangeState(CharacterState newState)
    {
        if (currentState == newState && newState == CharacterState.Attack)
        {
            // Trong trạng thái Attack, kích hoạt hoạt ảnh tấn công liên tục
            animator.SetTrigger("Attack");
            damageZone.BeginAttack();
            return;
        }

        // Kết thúc trạng thái cũ
        switch (currentState)
        {
            case CharacterState.Normal:
                damageZone.EndAttack();
                break;
            case CharacterState.Attack:
                damageZone.EndAttack();
                break;
        }

        // Bắt đầu trạng thái mới
        switch (newState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                animator.SetTrigger("Attack");
                damageZone.BeginAttack();
                break;
            case CharacterState.Die:
                animator.SetTrigger("Die");

                if (!expAdded)
                {
                    QuestManager questManager = FindObjectOfType<QuestManager>();
                    if (questManager != null)
                    {
                        questManager.UpdateKillCount(enemyName);
                        expAdded = true;
                    }
                }

                Destroy(gameObject, 3f);
                break;
        }

        currentState = newState;
    }
}