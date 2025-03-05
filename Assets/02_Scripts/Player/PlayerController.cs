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
        HandleFalling();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 입력 벡터
        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (inputDirection.magnitude > 0)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // 10f는 회전 속도 조절값

            currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
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
            animator.SetTrigger("Jump");

            StartCoroutine(PerformJump());
        }
    }

    IEnumerator PerformJump()
    {
        yield return new WaitForSeconds(0.3f); // 애니메이션 준비 동작 대기

        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        isGrounded = false;
        animator.SetBool("isGrounded", false);

        isFalling = false;
        animator.SetBool("isFalling", false);
    }

    private void HandleFalling()
    {
        if (!isGrounded && rb.velocity.y < 0)
        {
            if (!isFalling)
            {
                isFalling = true;
                animator.SetBool("isFalling", true);
            }
        }
        else
        {
            if (isFalling)
            {
                isFalling = false;
                animator.SetBool("isFalling", false);
            }
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
