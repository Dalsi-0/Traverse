using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody PlayerRigidbody { get; private set; }
    private Animator animator;
    private CapsuleCollider capsuleCollider;

    private float acceleration = 10f;
    private float deceleration = 5f;
    private float maxSpeed = 10f;
    private float jumpForce = 7f;
    private float staminaDrainRate = 10f;

    private Vector3 currentVelocity = Vector3.zero;

    public bool IsGrounded { get; private set; }
    public bool IsFalling { get; private set; }

    private float jumpTimeout = 1f;
    private float jumpTimeoutDelta = 0f;
    private float fallTimeout = 1f;
    private float fallTimeoutDelta = 0f;

    private bool isBowEquipped = false; // 활 장착 상태 변수
    private bool isCharging = false;    // 활 차징 상태 변수

    private Vector2 targetDir;
    private Vector2 smoothDir; // 부드러운 이동을 위한 변수
    private float smoothTime = 0.1f; // 부드럽게 변환되는 시간

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

    private PhysicMaterial fallPhysicMaterial;
    private PhysicMaterial groundPhysicMaterial;

    private PlayerInput playerInput; // PlayerInput 스크립트를 참조
    private Player player; // PlayerInput 스크립트를 참조

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        capsuleCollider = transform.GetComponent<CapsuleCollider>();
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

        groundPhysicMaterial = new PhysicMaterial
        {
            frictionCombine = PhysicMaterialCombine.Average,
            bounceCombine = PhysicMaterialCombine.Average,
            staticFriction = 0.6f,
            dynamicFriction = 0.6f,
        };

        fallPhysicMaterial = new PhysicMaterial
        {
            frictionCombine = PhysicMaterialCombine.Minimum, 
            bounceCombine = PhysicMaterialCombine.Minimum,
            staticFriction = 0f,
            dynamicFriction = 0f,
            bounciness = 0f 
        };

      //  capsuleCollider.material = groundPhysicMaterial;
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
        HandleChargingState();
    }

    private void FixedUpdate()
    {
        Move();
        HandleFalling();
    }

    private void Move()
    {
        targetDir = playerInput.GetMoveDirection(); // moveAction에 할당된 값

        if (targetDir != Vector2.zero)
        {
            // 부드럽게 이동 방향을 변화시킴
            smoothDir = Vector2.Lerp(smoothDir, targetDir, smoothTime); // Lerp로 부드럽게 전환
        }
        else
        {
            smoothDir = targetDir;
        }

        // 이동 방향 처리
        float moveX = smoothDir.x * 1f;  // 이동 입력값
        float moveZ = smoothDir.y * 1f;  // 이동 입력값
        
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        if (isBowEquipped)  // 활 장착 상태일 때
        {
            Vector3 moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

            if (moveDirection.magnitude > 0)
            {
                // 캐릭터가 카메라 정면을 바라보도록 회전
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                // 활 차징 상태라면 속도를 추가 감소
                float speedModifier = isCharging ? 0.2f : 0.3f;
                currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * (maxSpeed * speedModifier), acceleration * Time.deltaTime);
            }
            else
            {
                // 입력이 없을 때도 캐릭터를 카메라 정면으로 회전
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
            }

            PlayerRigidbody.velocity = new Vector3(currentVelocity.x, PlayerRigidbody.velocity.y, currentVelocity.z);

            float speedRatio = currentVelocity.magnitude / (maxSpeed * (isCharging ? 0.3f : 0.5f));
            animator.SetFloat(animIDSpeed, Mathf.Clamp(speedRatio, 0f, 1f));

            animator.SetFloat(animIDMoveX, moveX);
            animator.SetFloat(animIDMoveZ, moveZ);
        }
        else  // 활을 장착하지 않았을 때는 기존 방식
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

            PlayerRigidbody.velocity = new Vector3(currentVelocity.x, PlayerRigidbody.velocity.y, currentVelocity.z);
            float speedRatio = currentVelocity.magnitude / maxSpeed;
            animator.SetFloat(animIDSpeed, Mathf.Clamp(speedRatio, 0f, 1f));
        }
    }

    private void HandleChargingState()
    {   
        if (isCharging)  // 활 차징 중일 때
        {
            if (player.stamina > 0)
            {
                DrainStamina();
            }
            else
            {
                StopCharging();
            }
        }
    }

    private void DrainStamina()
    {
        // 스태미나가 소모되도록 처리
        if (player.ConsumeStamina(staminaDrainRate * Time.deltaTime))
        {
        }
        else
        {
            // 스태미나가 부족하면 차징을 취소
            StopCharging();
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
        if (IsGrounded && jumpTimeoutDelta <= 0.0f && player.ConsumeStamina(30))
        {
            animator.SetTrigger(animIDJump);
            PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, jumpForce, PlayerRigidbody.velocity.z);
            IsGrounded = false;
            animator.SetBool(animIDGrounded, false);

            IsFalling = false;
            animator.SetBool(animIDIsFalling, false);

            if (isBowEquipped)
            {
                isBowEquipped = false;
                isCharging = false; 
            }
        }
    }

    private void HandleFalling()
    {
        if (!IsGrounded && PlayerRigidbody.velocity.y < 0 && jumpTimeoutDelta < 0.0f)
        {
            jumpTimeoutDelta = jumpTimeout;
            if (!IsFalling)
            {
                IsFalling = true;
                animator.SetBool(animIDIsFalling, true);
            }

            if (isBowEquipped)
            {
                isBowEquipped = false;
                isCharging = false; 
                animator.ResetTrigger(animIDUnequipBow); // 트리거 강제 해제
            }
        }
        else
        {
            fallTimeoutDelta = fallTimeout;
            if (IsFalling)
            {
                IsFalling = false;
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

    // 활 해제
    private void UnequipBow()
    {
        if (isBowEquipped)
        {
            if (isCharging)
            {
                isCharging = false;
                animator.SetTrigger(animIDReleaseBow);
            }

            isBowEquipped = false;
            animator.SetTrigger(animIDUnequipBow);
        }
    }

    // 활 차징 (좌클릭 시작)
    private void StartCharging()
    {
        if (isBowEquipped && !isCharging && player.stamina > (staminaDrainRate * 0.5f))
        {
            isCharging = true;
            animator.SetTrigger(animIDDrawBow);
        }
    }

    // 활 발사 (좌클릭 해제)
    private void StopCharging()
    {
        if (isBowEquipped && isCharging)
        {
            isCharging = false;
            animator.SetTrigger(animIDReleaseBow);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
            capsuleCollider.material = groundPhysicMaterial;
            animator.SetBool(animIDGrounded, true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
            capsuleCollider.material = fallPhysicMaterial;
            animator.SetBool(animIDGrounded, false);
        }
    }
}