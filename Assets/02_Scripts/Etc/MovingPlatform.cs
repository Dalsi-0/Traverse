using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // �̵� ����
    public float moveDistance = 5f; // �̵� �Ÿ�
    public float moveSpeed = 2f; // �̵� �ӵ�

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // �ʱ� ��ġ ����
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
            other.transform.SetParent(transform); // �÷��̾ ������ �ڽ����� ����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null); // �÷��̾ �ٽ� ���� ���·� ����
        }
    }
}
