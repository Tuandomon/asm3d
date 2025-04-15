using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 5f;
    public float sprintSpeed = 10f; // Tốc độ chạy nhanh
    public Vector3 movementVelocity;
    public PlayerInput playerInput;
    public AudioSource audioSource;  // Thành phần AudioSource để phát âm thanh
    public AudioClip hitSound;       // Âm thanh khi bị đánh

    // Animation
    public Animator animator;

    public DamageZone damageZone;

    public Health health;

    // Trạng thái của nhân vật
    public enum CharacterState
    {
        Normal,
        Attack,
        Die
    }
    public CharacterState currentState; // Trạng thái hiện tại

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();  // Lấy AudioSource trên nhân vật
        }
    }

    public void TakeDamage()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);  // Phát âm thanh bị đánh
        }

        // Thêm logic xử lý mất máu hoặc hiệu ứng tại đây
        Debug.Log("Nhân vật bị đánh!");
    }

    void FixedUpdate()
    {
        SimulateFixedUpdate(); // Chuyển toàn bộ logic sang phương thức công khai mới
    }

    // Phương thức công khai mới chứa logic của FixedUpdate
    public void SimulateFixedUpdate()
    {
        if (health.currentHP <= 0)
        {
            ChangeState(CharacterState.Die);
            return;
        }

        switch (currentState)
        {
            case CharacterState.Normal:
                CalculateMovement();
                characterController.Move(movementVelocity);
                break;

            case CharacterState.Attack:
                movementVelocity = Vector3.zero;
                characterController.Move(movementVelocity);
                break;
        }
    }

    void CalculateMovement()
    {
        if (playerInput.attackInput)
        {
            ChangeState(CharacterState.Attack);
            animator.SetFloat("Speed", 0);
            return;
        }

        float horizontalInput = playerInput.horizontalInput;
        float verticalInput = playerInput.verticalInput;

        movementVelocity = new Vector3(horizontalInput, 0, verticalInput);
        movementVelocity.Normalize();
        movementVelocity = Camera.main.transform.TransformDirection(movementVelocity);
        movementVelocity.y = 0;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movementVelocity *= sprintSpeed * Time.deltaTime; // Tốc độ chạy nhanh
        }
        else
        {
            movementVelocity *= speed * Time.deltaTime; // Tốc độ bình thường
        }

        animator.SetFloat("Speed", movementVelocity.magnitude);
        if (movementVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementVelocity);
        }
    }

    private void ChangeState(CharacterState newState)
    {
        playerInput.attackInput = false;

        switch (currentState)
        {
            case CharacterState.Normal:
                break;

            case CharacterState.Attack:
                break;
        }

        switch (newState)
        {
            case CharacterState.Normal:
                animator.SetFloat("Speed", 0);
                break;

            case CharacterState.Attack:
                animator.SetTrigger("Attack");
                break;

            case CharacterState.Die:
                animator.SetTrigger("Die");
                break;
        }

        currentState = newState;
    }

    public void OnAttack1End()
    {
        ChangeState(CharacterState.Normal);
    }
    public void BeginAttack()
    {
        damageZone.BeginAttack();
    }
    public void EndAttack()
    {
        damageZone.EndAttack();
    }

    public void IncreaseCharacterDamage(int amount)
    {
        damageZone.IncreaseDamage(amount);
    }
}