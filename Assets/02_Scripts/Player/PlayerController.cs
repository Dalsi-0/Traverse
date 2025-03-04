using UnityEngine;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        HandleJump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);

            // 가속
            currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // 감속 
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);

        // 현재 속도 비율에 따라 애니메이션 속도 조절
        float speedRatio = currentVelocity.magnitude / maxSpeed;
        animator.SetFloat("speed", Mathf.Clamp(speedRatio, 0f, 1f));
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            animator.SetTrigger("Jump");
            isGrounded = false;
            animator.SetBool("isGrounded", false);
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
}
