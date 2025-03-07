using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounceForce = 10f; // ƨ��� ��

    PlayerController playerController;

    private void Start()
    {
        playerController = PlayerManager.Instance.GetPlayerReferences().PlayerController;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾ ���� ���̰ų� ���� ���� ���� ƨ�ܳ���
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.PlayerRigidbody.velocity = Vector3.zero;

            // �������� ���� "���� ����(transform.up)"���� ���� ��
            Vector3 bounceDirection = transform.up;

            playerController.PlayerRigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }
}
