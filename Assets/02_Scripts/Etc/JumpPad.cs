using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounceForce = 10f; // 튕기는 힘

    PlayerController playerController;

    private void Start()
    {
        playerController = PlayerManager.Instance.GetPlayerReferences().PlayerController;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어가 점프 중이거나 낙하 중일 때만 튕겨내기
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.PlayerRigidbody.velocity = Vector3.zero;

            // 점프대의 현재 "위쪽 방향(transform.up)"으로 힘을 줌
            Vector3 bounceDirection = transform.up;

            playerController.PlayerRigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }
}
