using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform target; //muc tieu

    public float radius = 10f; //ban kinh tim kiem muc tieu
    public Vector3 originalePosition; //vi tri ban dau
    public float maxDistance = 50f; //khoang cach toi da

    public Animator animator;

    public DamageZone damageZone;

    public Health health;

    public float attackCooldown = 2f; // Thời gian chờ giữa các lần tấn công
    private float lastAttackTime = -Mathf.Infinity; // Thời điểm lần tấn công cuối cùng

    //state machine
    public enum CharacterState
    {
        Normal,
        Attack,
        Die
    }
    public CharacterState currentState; //trang thai hien tai

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalePosition = transform.position;
    }

    void Update()
    {
        if (health.currentHP <= 0)
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

        // Khoảng cách từ vị trí hiện tại đến vị trí ban đầu
        var distanceToOriginal = Vector3.Distance(originalePosition, target.position);
        // Khoảng cách từ vị trí hiện tại đến mục tiêu
        var distance = Vector3.Distance(target.position, transform.position);

        if (distance <= radius && distanceToOriginal <= maxDistance)
        {
            // Di chuyển đến mục tiêu
            navMeshAgent.SetDestination(target.position);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            distance = Vector3.Distance(target.position, transform.position);
            if (distance < 2f && Time.time >= lastAttackTime + attackCooldown) // Kiểm tra thời gian chờ
            {
                // Tấn công
                ChangeState(CharacterState.Attack);
                lastAttackTime = Time.time; // Cập nhật thời điểm tấn công cuối cùng
            }
        }

        if (distance > radius || distanceToOriginal > maxDistance)
        {
            // Quay về vị trí ban đầu
            navMeshAgent.SetDestination(originalePosition);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            // Chuyển sang trạng thái đứng yên
            distance = Vector3.Distance(originalePosition, transform.position);
            if (distance < 1f)
            {
                animator.SetFloat("Speed", 0);
            }

            // Bình thường
            ChangeState(CharacterState.Normal);
        }
    }

    // Chuyển đổi trạng thái
    private void ChangeState(CharacterState newState)
    {
        // Exit current state
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                break;
        }

        // Enter new state
        switch (newState)
        {
            case CharacterState.Normal:
                damageZone.EndAttack();
                break;
            case CharacterState.Attack:
                animator.SetTrigger("Attack");
                damageZone.BeginAttack();
                break;
            case CharacterState.Die:
                animator.SetTrigger("Die");
                Destroy(gameObject, 3f);
                break;
        }

        // Update current state
        currentState = newState;
    }
}

