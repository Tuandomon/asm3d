using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 5f;
    public float sprintSpeed = 10f; // Bổ sung: Tốc độ chạy nhanh
    public Vector3 movementVelocity;
    public PlayerInput playerInput;
    public AudioSource audioSource;  // Thành phần AudioSource để phát âm thanh
    public AudioClip hitSound;      // Âm thanh khi bị đánh

    // Animation
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

    // Start is called before the first frame update
    void Start()
    {
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();  // Lấy AudioSource trên nhân vật
            }
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health.currentHP <= 0)
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
        //characterController.Move(movementVelocity);
    }

    void CalculateMovement()
    {
        if (playerInput.attackInput)
        {
            ChangeState(CharacterState.Attack);
            animator.SetFloat("Speed", 0);
            return;
        }

        // Lấy thông tin đầu vào từ người chơi
        float horizontalInput = playerInput.horizontalInput;
        float verticalInput = playerInput.verticalInput;

        // Tạo vector chuyển động dựa trên đầu vào
        movementVelocity = new Vector3(horizontalInput, 0, verticalInput);

        // Chuẩn hóa vector để đảm bảo tốc độ di chuyển nhất quán
        movementVelocity.Normalize();

        // Chuyển đổi vector theo hướng camera để nhân vật luôn di chuyển theo góc nhìn của người chơi
        movementVelocity = Camera.main.transform.TransformDirection(movementVelocity);

        // Loại bỏ thành phần y để giữ nhân vật di chuyển trên mặt phẳng xz
        movementVelocity.y = 0;

        // Tính toán vận tốc thực tế của nhân vật
        //movementVelocity *= speed * Time.deltaTime;

        // Kiểm tra phím Shift để thay đổi tốc độ 
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        { 
            movementVelocity *= sprintSpeed * Time.deltaTime; // Sử dụng tốc độ chạy nhanh 
        } 
        else 
        { 
            movementVelocity *= speed * Time.deltaTime; // Sử dụng tốc độ bình thường
        }
        // Cập nhật animation
        animator.SetFloat("Speed", movementVelocity.magnitude);
        // Xoay hướng của nhân vật theo hướng di chuyển
        if (movementVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementVelocity);
        }
    }

    //chuyen doi trang thai
    private void ChangeState(CharacterState newState)
    {
        //xoa cache
        playerInput.attackInput = false;
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
                animator.SetFloat("Speed", 0);
                break;
            case CharacterState.Attack:
                animator.SetTrigger("Attack");
                break;
            case CharacterState.Die:
                animator.SetTrigger("Die");
                break;
        }

        //update current state
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

    /*public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (currentHP <= 0)
        {
           ChangeState(CharacterState.Die);
        }
    }*/
}