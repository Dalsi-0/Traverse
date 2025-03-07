using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounceForce = 10f; // 튕기는 힘
    public float height = 5f; // 튕겨내는 높이

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어가 점프 중이거나 낙하 중일 때만 튕겨내기
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = PlayerManager.Instance.GetPlayerReferences().PlayerController;

            // 플레이어가 점프 중이거나 낙하 중일 때만 처리
            if (playerController != null && playerController.PlayerRigidbody.velocity.y < 0)
            {
                // 점프대 위에 있는 모든 오브젝트 튕겨내기
                Collider[] objectsAbove = Physics.OverlapBox(transform.position, new Vector3(5f, 10f, 5f), Quaternion.identity);
                foreach (Collider obj in objectsAbove)
                {
                    Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // 위로 튕겨내는 힘을 추가
                        rb.AddForce(Vector3.up * height + Vector3.up * bounceForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
