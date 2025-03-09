using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounceForce = 10f;

    PlayerController playerController;

    private void Start()
    {
        playerController = PlayerManager.Instance.GetPlayerReferences().PlayerController;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           // playerController.PlayerRigidbody.AddForce(transform.up * bounceForce, ForceMode.Impulse);
        }
    }
}
