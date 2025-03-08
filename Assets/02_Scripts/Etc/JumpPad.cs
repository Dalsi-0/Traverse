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
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 bounceDirection = transform.up;

            playerController.PlayerRigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }
}
