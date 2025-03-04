using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private bool isGrounded;
    private Vector3 lastMoveDirection;

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

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection.magnitude > 0)
        {
            // ������ �������� ȸ��
            transform.rotation = Quaternion.LookRotation(moveDirection);
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

            // �޸��� ����
            animator.SetBool("isRunning", true);

            // �ް��� ���� ��ȯ üũ
            if (Vector3.Angle(lastMoveDirection, moveDirection) > 90)
            {
                animator.SetBool("isStopping", true);
            }
            else
            {
                animator.SetBool("isStopping", false);
            }

            lastMoveDirection = moveDirection;
        }
        else
        {
            // �޸��ٰ� ���� ��쿡�� isStopping Ȱ��ȭ
            if (animator.GetBool("isRunning"))
            {
                animator.SetBool("isStopping", true);
            }
            else
            {
                animator.SetBool("isStopping", false);
            }

            animator.SetBool("isRunning", false);
        }
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
