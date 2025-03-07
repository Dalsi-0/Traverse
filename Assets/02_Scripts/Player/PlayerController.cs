using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    private float acceleration = 5f;
    private float deceleration = 5f;
    private float maxSpeed = 6f;
    private float jumpForce = 7f;

    private Vector3 currentVelocity = Vector3.zero;

    private bool isGrounded;
    private bool isFalling;

    private float jumpTimeout = 1f;
    private float jumpTimeoutDelta = 0f;
    private float fallTimeout = 1f;
    private float fallTimeoutDelta = 0f;

    private bool isBowEquipped = false; // Ȱ ���� ���� ����
    private bool isCharging = false;    // Ȱ ��¡ ���� ����

    private Vector2 targetDir;
    private Vector2 smoothDir; // �ε巯�� �̵��� ���� ����
    private float smoothTime = 0.1f; // �ε巴�� ��ȯ�Ǵ� �ð�

    // animation IDs
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDMoveX;
    private int animIDMoveZ;
    private int animIDIsFalling;
    private int animIDDrawBow;
    private int animIDReleaseBow;
    private int animIDEquipBow;
    private int animIDUnequipBow;

    private PlayerInput playerInput; // PlayerInput ��ũ��Ʈ�� ����
    private Player player; // PlayerInput ��ũ��Ʈ�� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerInput = PlayerManager.Instance.GetPlayerReferences().PlayerInput;
        player = PlayerManager.Instance.GetPlayerReferences().Player;

        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
        animIDSpeed = Animator.StringToHash("speed");
        animIDGrounded = Animator.StringToHash("isGrounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDMoveX = Animator.StringToHash("moveX");
        animIDMoveZ = Animator.StringToHash("moveZ");
        animIDIsFalling = Animator.StringToHash("isFalling");
        animIDDrawBow = Animator.StringToHash("DrawBow");
        animIDReleaseBow = Animator.StringToHash("ReleaseBow");
        animIDEquipBow = Animator.StringToHash("EquipBow");
        animIDUnequipBow = Animator.StringToHash("UnequipBow");

    }
    private void OnEnable()
    {
        playerInput.jumpEvent += HandleJump;
        playerInput.leftClickStartedEvent += StartCharging;
        playerInput.leftClickCanceledEvent += StopCharging;
        playerInput.rightClickStartedEvent += EquipBow;
        playerInput.rightClickCanceledEvent += UnequipBow;
    }

    private void OnDisable()
    {
        playerInput.jumpEvent -= HandleJump;
        playerInput.rightClickStartedEvent -= EquipBow;
        playerInput.rightClickCanceledEvent -= UnequipBow;
        playerInput.leftClickStartedEvent -= StartCharging;
        playerInput.leftClickCanceledEvent -= StopCharging;
    }

    private void Update()
    {
        HandleTimeout();
    }

    private void FixedUpdate()
    {
        Move();
        HandleFalling();
    }

    private void Move()
    {
        targetDir = playerInput.GetMoveDirection(); // moveAction�� �Ҵ�� ��

        if (targetDir != Vector2.zero)
        {
            // �ε巴�� �̵� ������ ��ȭ��Ŵ
            smoothDir = Vector2.Lerp(smoothDir, targetDir, smoothTime); // Lerp�� �ε巴�� ��ȯ
        }
        else
        {
            smoothDir = targetDir;
        }

        // �̵� ���� ó��
        float moveX = smoothDir.x * 1f;  // �̵� �Է°�
        float moveZ = smoothDir.y * 1f;  // �̵� �Է°�
        
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        if (isBowEquipped)  // Ȱ ���� ������ ��
        {
            Vector3 moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

            if (moveDirection.magnitude > 0)
            {
                // ĳ���Ͱ� ī�޶� ������ �ٶ󺸵��� ȸ��
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                // Ȱ ��¡ ���¶�� �ӵ��� �߰� ����
                float speedModifier = isCharging ? 0.2f : 0.3f;
                currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * (maxSpeed * speedModifier), acceleration * Time.deltaTime);
            }
            else
            {
                // �Է��� ���� ���� ĳ���͸� ī�޶� �������� ȸ��
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
            }

            rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);

            float speedRatio = currentVelocity.magnitude / (maxSpeed * (isCharging ? 0.3f : 0.5f));
            animator.SetFloat(animIDSpeed, Mathf.Clamp(speedRatio, 0f, 1f));

            animator.SetFloat(animIDMoveX, moveX);
            animator.SetFloat(animIDMoveZ, moveZ);
        }
        else  // Ȱ�� �������� �ʾ��� ���� ���� ���
        {
            Vector3 moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

            if (moveDirection.magnitude > 0)
            {
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
            animator.SetFloat(animIDSpeed, Mathf.Clamp(speedRatio, 0f, 1f));
        }
    }

    private void HandleTimeout()
    {
        if (jumpTimeoutDelta > 0)
        {
            jumpTimeoutDelta -= Time.deltaTime;
        }

        if (fallTimeoutDelta >= 0.0f)
        {
            fallTimeoutDelta -= Time.deltaTime;
        }
    }

    private void HandleJump()
    {
        if (isGrounded && jumpTimeoutDelta <= 0.0f && player.ConsumeStamina(30))
        {
            animator.SetTrigger(animIDJump);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
            animator.SetBool(animIDGrounded, false);

            isFalling = false;
            animator.SetBool(animIDIsFalling, false);

            // �����ϸ� Ȱ ���� ����
            if (isBowEquipped)
            {
                isBowEquipped = false;
                isCharging = false; // ��¡ ���� ����
            }
        }
    }

    private void HandleFalling()
    {
        if (!isGrounded && rb.velocity.y < 0 && jumpTimeoutDelta < 0.0f)
        {
            jumpTimeoutDelta = jumpTimeout;
            if (!isFalling)
            {
                isFalling = true;
                animator.SetBool(animIDIsFalling, true);
            }

            // ���� �� Ȱ ���� ����
            if (isBowEquipped)
            {
                isBowEquipped = false;
                isCharging = false; // ��¡ ���� ����
                animator.ResetTrigger(animIDUnequipBow); // Ʈ���� ���� ����
            }
        }
        else
        {
            fallTimeoutDelta = fallTimeout;
            if (isFalling)
            {
                isFalling = false;
                animator.SetBool(animIDIsFalling, false);
            }
        }
    }

    private void EquipBow()
    {
        if (!isBowEquipped)
        {
            isBowEquipped = true;
            animator.SetTrigger(animIDEquipBow);
        }
    }

    // Ȱ ����
    private void UnequipBow()
    {
        if (isBowEquipped)
        {
            // Ȱ ��¡ ���¶�� �ݵ�� Ȱ �߻� �ִϸ��̼��� �����Ͽ� �ڿ������� ����
            if (isCharging)
            {
                isCharging = false;
                animator.SetTrigger(animIDReleaseBow);
            }

            isBowEquipped = false;
            animator.SetTrigger(animIDUnequipBow);
        }
    }

    // Ȱ ��¡ (��Ŭ�� ����)
    private void StartCharging()
    {
        if (isBowEquipped)
        {
            isCharging = true;
            animator.SetTrigger(animIDDrawBow);
        }
    }

    // Ȱ �߻� (��Ŭ�� ����)
    private void StopCharging()
    {
        if (isBowEquipped)
        {
            isCharging = false;
            animator.SetTrigger(animIDReleaseBow);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool(animIDGrounded, true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool(animIDGrounded, false);
        }
    }
}