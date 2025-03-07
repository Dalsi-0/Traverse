using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // 이동 방향
    public float moveDistance = 5f; // 이동 거리
    public float moveSpeed = 2f; // 이동 속도

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // 초기 위치 저장
    }

    private void FixedUpdate()
    {
        float pingPongValue = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        transform.position = startPosition + moveDirection.normalized * pingPongValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform); // 플레이어를 발판의 자식으로 설정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null); // 플레이어를 다시 원래 상태로 복귀
        }
    }
}
