using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody PlayerRigidbody { get; private set; }
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    private Camera mainCam;

    private float acceleration = 10f;
    private float deceleration = 10f;
    private float maxSpeed = 10f;
    private float jumpForce = 7f;
    private float staminaDrainRate = 10f;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector2 targetDir;
    private Vector2 smoothDir;
    private float smoothTime = 0.1f;

    private float jumpTimeout = 1f;
    private float jumpTimeoutDelta = 0f;
    private float fallTimeout = 0.1f;
    private float fallTimeoutDelta = 0f;

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

    private PlayerInput playerInput;
    private Player player;

    private bool isBowEquipped = false; 
    private bool isCharging = false;    
    private bool isGrounded;
    private bool isFalling;
    private bool isSlope;




    [SerializeField] Transform groundCheck;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] private LayerMask groundLayer;
    private const float RAY_DISTANCE = 2f;
    private RaycastHit slopeHit;
    private float maxSlopeAngle = 40f; // 경사각도 최대치
    private float slopeDownForce = 80f;
    private Vector3 spineRotationOffset = new Vector3(0, 70, 0);


    public Transform spine; // 아바타의 상체

    public float rotationSpeed = 5f; // 회전 속도 조절

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        capsuleCollider = transform.GetComponent<CapsuleCollider>();
        mainCam = Camera.main;

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

        capsuleCollider.material = groundPhysicMaterial;

        spine = animator.GetBoneTransform(HumanBodyBones.Spine); // 상체 transform 가져오기
        if (spine == null)
        {
            Debug.LogError("Spine Transform을 찾을 수 없습니다!");
        }
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

    private void LateUpdate()
    {
        RotateUpperBody();
    }

    private void RotateUpperBody()
    {
        if (spine == null || !isBowEquipped) return; 

        Vector3 targetDirection = mainCam.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation *= Quaternion.Euler(spineRotationOffset);

        spine.rotation = targetRotation;
    }

    private void Move()
    {
        targetDir = playerInput.GetMoveDirection();

        smoothDir = targetDir != Vector2.zero
                    ? Vector2.Lerp(smoothDir, targetDir, smoothTime)
                    : targetDir;

        Vector3 cameraForward = mainCam.transform.forward;
        Vector3 cameraRight = mainCam.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        isSlope = IsOnSlope();
        IsGrounded();

        Vector3 gravity = Vector3.down * Mathf.Abs(PlayerRigidbody.velocity.y);
        
        Vector3 moveDirection = (cameraForward * smoothDir.y + cameraRight * smoothDir.x).normalized;

        if (isGrounded && isSlope)
        {
            moveDirection = AdjustDirectionToSlope(moveDirection);
            PlayerRigidbody.AddForce(-slopeHit.normal * slopeDownForce, ForceMode.Force);
        }

        if (isBowEquipped)
        {
            HandleBowMovement(moveDirection);
        }
        else
        {
            HandleRegularMovement(moveDirection);
        }
    }

    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public void IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x*0.5f, 0.2f, transform.lossyScale.z * 0.5f);
        
        isGrounded = Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity, groundLayer);
        
        animator.SetBool(animIDGrounded, isGrounded); 
    }

    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, RAY_DISTANCE, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle;
        }
        return false;
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxSize = new Vector3(transform.lossyScale.x * 0.5f, 0.2f, transform.lossyScale.z * 0.5f);
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }*/

    private void HandleRegularMovement(Vector3 moveDirection)
    {
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
        animator.SetFloat(animIDSpeed, currentVelocity.magnitude / maxSpeed);
    }

    private void HandleBowMovement(Vector3 moveDirection)
    {
        Vector3 cameraForward = mainCam.transform.forward;
        cameraForward.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

        if (moveDirection.magnitude > 0)
        {
            float speedModifier = isCharging ? 0.2f : 0.3f;
            currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * (maxSpeed * speedModifier), acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        PlayerRigidbody.velocity = new Vector3(currentVelocity.x, PlayerRigidbody.velocity.y, currentVelocity.z);
        float speedRatio = currentVelocity.magnitude / (maxSpeed * (isCharging ? 0.3f : 0.5f));
        animator.SetFloat(animIDSpeed, Mathf.Clamp(speedRatio, 0f, 1f));
        animator.SetFloat(animIDMoveX, smoothDir.x);
        animator.SetFloat(animIDMoveZ, smoothDir.y);
    }

    private void HandleChargingState()
    {
        if (isCharging)
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
        if (!player.ConsumeStamina(staminaDrainRate * Time.deltaTime))
        {
            StopCharging();
        }
    }

    private void HandleTimeout()
    {
        if (jumpTimeoutDelta > 0)
        {
            jumpTimeoutDelta -= Time.deltaTime;
        }

        if (!isGrounded && fallTimeoutDelta >= 0.0f)
        {
            fallTimeoutDelta -= Time.deltaTime;
        }
        else if (isGrounded)
        {
            fallTimeoutDelta = fallTimeout;
        }
    }

    private void HandleJump()
    {
        if (isGrounded && jumpTimeoutDelta <= 0f && player.ConsumeStamina(30))
        {
            jumpTimeoutDelta = jumpTimeout;
            animator.SetTrigger(animIDJump);

            float jumpPower = isSlope ? jumpForce + slopeDownForce *0.065f : jumpForce;
            PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, jumpPower, PlayerRigidbody.velocity.z);
            
            if (isBowEquipped)
            {
                isBowEquipped = false;
                isCharging = false;
            }
        }
    }

    private void HandleFalling()
    {
        if (!isGrounded && fallTimeoutDelta < 0.0f)
        {
            jumpTimeoutDelta = jumpTimeout;
            if (!isFalling)
            {
                isFalling = true;
                animator.SetBool(animIDIsFalling, true);
            }

            if (isBowEquipped)
            {
                isBowEquipped = false;
                isCharging = false;
                animator.ResetTrigger(animIDUnequipBow);
            }
        }
        else
        {
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

    private void StartCharging()
    {
        if (isBowEquipped && !isCharging && player.stamina > (staminaDrainRate * 0.5f))
        {
            isCharging = true;
            animator.SetTrigger(animIDDrawBow);
        }
    }

    private void StopCharging()
    {
        if (isCharging)
        {
            isCharging = false;
            animator.SetTrigger(animIDReleaseBow);
        }
    }

}
