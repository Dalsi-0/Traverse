using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    public float acceleration = 5f;
    public float deceleration = 5f;
    public float maxSpeed = 6f;
    private Vector3 moveDirection;
    private Vector3 currentVelocity = Vector3.zero;

    public float jumpForce = 7f;
    private bool isGrounded;
    private bool isFalling;

    public float jumpTimeout = 1f;
    private float jumpTimeoutDelta = 0f;
    public float fallTimeout = 1f;
    private float fallTimeoutDelta = 0f;

    private bool isBowEquipped = false; // Ȱ ���� ���� ����
    private bool isCharging = false;    // Ȱ ��¡ ���� ����


    // animation IDs
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();


        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    void Update()
    {
        HandleJump();
        HandleBowEquip(); // Ȱ ���� ó��
        HandleBowCharge(); // Ȱ ��¡ ó��
    }

    private void FixedUpdate()
    {
        Move();
        HandleFalling();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");  // �¿� �̵�
        float moveZ = Input.GetAxis("Vertical");    // �յ� �̵�

        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (isBowEquipped)  // Ȱ ���� ������ ��
        {
            if (inputDirection.magnitude > 0)
            {
                Vector3 cameraForward = Camera.main.transform.forward;
                Vector3 cameraRight = Camera.main.transform.right;

                cameraForward.y = 0f;
                cameraRight.y = 0f;

                moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

                // ī�޶� ������ �ٶ󺸵��� ȸ��
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                // Ȱ ��¡ ���¶�� �ӵ��� �߰� ����
                float speedModifier = isCharging ? 0.3f : 0.5f;
                currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * (maxSpeed * speedModifier), acceleration * Time.deltaTime);
            }
            else
            {
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
            }

            rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);

            float speedRatio = currentVelocity.magnitude / (maxSpeed * (isCharging ? 0.3f : 0.5f));
            animator.SetFloat("speed", Mathf.Clamp(speedRatio, 0f, 1f));

            animator.SetFloat("moveX", moveX);
            animator.SetFloat("moveZ", moveZ);
        }
        else  // Ȱ�� �������� �ʾ��� ���� �⺻ �̵�
        {
            if (inputDirection.magnitude > 0)
            {
                Vector3 cameraForward = Camera.main.transform.forward;
                Vector3 cameraRight = Camera.main.transform.right;

                cameraForward.y = 0f;
                cameraRight.y = 0f;

                moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * maxSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
            }

            rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);

            float speedRatio = currentVelocity.magnitude / maxSpeed;
            animator.SetFloat("speed", Mathf.Clamp(speedRatio, 0f, 1f));
        }
    }

    void HandleJump()
    {
        if (jumpTimeoutDelta > 0)
        {
            jumpTimeoutDelta -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpTimeoutDelta <= 0.0f)
        {
            animator.SetTrigger("Jump");
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
            animator.SetBool("isGrounded", false);

            isFalling = false;
            animator.SetBool("isFalling", false);

            // �����ϸ� Ȱ ���� ����
            if (isBowEquipped)
            {
                UnequipBow();
            }
        }
    }

    private void HandleFalling()
    {
        if (fallTimeoutDelta >= 0.0f)
        {
            fallTimeoutDelta -= Time.deltaTime;
        }

        if (!isGrounded && rb.velocity.y < 0 && jumpTimeoutDelta < 0.0f)
        {
            jumpTimeoutDelta = jumpTimeout;
            if (!isFalling)
            {
                isFalling = true;
                animator.SetBool("isFalling", true);
            }

            // ���� �� Ȱ ���� ����
            if (isBowEquipped)
            {
                UnequipBow();
            }
        }
        else
        {
            fallTimeoutDelta = fallTimeout;
            if (isFalling)
            {
                isFalling = false;
                animator.SetBool("isFalling", false);
            }
        }
    }

    private void HandleBowEquip()
    {
        if (Input.GetMouseButtonDown(1)) // ��Ŭ�� �� Ȱ ����
        {
            EquipBow();
        }
        else if (Input.GetMouseButtonUp(1)) // ��Ŭ�� ���� �� Ȱ ����
        {
            UnequipBow();
        }
    }

    private void HandleBowCharge()
    {
        if (isBowEquipped)
        {
            if (Input.GetMouseButtonDown(0)) // ��Ŭ�� ���� �� Ȱ ��ο� �ִϸ��̼� ����
            {
                isCharging = true;
                animator.SetTrigger("DrawBow");
            }
            else if (Input.GetMouseButtonUp(0)) // ��Ŭ�� ���� �� ��¡ ����
            {
                isCharging = false;
                animator.SetTrigger("ReleaseBow");
            }
        }
    }

    private void EquipBow()
    {
        if (!isBowEquipped)
        {
            isBowEquipped = true;
            animator.SetTrigger("EquipBow");
        }
    }

    private void UnequipBow()   
    {
        if (isBowEquipped)
        {
            isBowEquipped = false;
            isCharging = false; // ��¡ ���� ����
            animator.SetTrigger("UnequipBow");
            animator.ResetTrigger("UnequipBow"); // Ʈ���� ���� ����
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }
}
