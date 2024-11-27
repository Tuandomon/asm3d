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
        if(health.currentHP <=0)
        {
            ChangeState(CharacterState.Die);
        }
        //xoay huong ve muc tieu
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
        //khoang cach tu vi tri hien tai den vi tri ban dau
        var distanceToOriginal = Vector3.Distance(originalePosition, target.position);
        //khoang cach tu vi tri hien tai den muc tieu
        var distance = Vector3.Distance(target.position, transform.position);
        if (distance <= radius && distanceToOriginal <= maxDistance)
        {
            //di chuyen den muc tieu
            navMeshAgent.SetDestination(target.position);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            distance = Vector3.Distance(target.position, transform.position);
            if (distance < 2f)
            {
                //tan cong
                ChangeState(CharacterState.Attack);
            }
        }

        if (distance > radius || distanceToOriginal > maxDistance)
        {
            //quay ve vi tri ban dau
            navMeshAgent.SetDestination(originalePosition);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            //chuyen sang trang thai dung yen
            distance = Vector3.Distance(originalePosition, transform.position);
            if (distance < 1f)
            {
                animator.SetFloat("Speed", 0);
            }

            //binh thuong
            ChangeState(CharacterState.Normal);
        }
    }

    //chuyen doi trang thai
    private void ChangeState(CharacterState newState)
    {
        //exit current state
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                break;
        }

        //enter new state
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
                Destroy(gameObject, 5f);
                break;
        }

        //update current state
        currentState = newState;
    }

    //di xung quanh vi tri ban dau: xem video buoi 6
}
