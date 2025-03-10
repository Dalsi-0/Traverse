using UnityEngine;

public class AutoMovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform startPoint;  // 시작 위치
    [SerializeField] private Transform endPoint;    // 끝 위치
    private Vector3 startPos;
    private Vector3 endPos;
    [SerializeField] private float speed = 2f;      // 이동 속도

    private void Start()
    {
        startPos = startPoint.position;
        endPos = endPoint.position;
        transform.position = startPos;
    }

    private void FixedUpdate()
    {
        float pingPongValue = Mathf.PingPong(Time.time * speed, 1);

        transform.position = Vector3.Lerp(startPos, endPos, pingPongValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
