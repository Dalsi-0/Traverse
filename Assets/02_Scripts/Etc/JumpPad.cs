using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounceForce = 10f; // ƨ��� ��
    public float height = 5f; // ƨ�ܳ��� ����

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾ ���� ���̰ų� ���� ���� ���� ƨ�ܳ���
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = PlayerManager.Instance.GetPlayerReferences().PlayerController;

            // �÷��̾ ���� ���̰ų� ���� ���� ���� ó��
            if (playerController != null && playerController.PlayerRigidbody.velocity.y < 0)
            {
                // ������ ���� �ִ� ��� ������Ʈ ƨ�ܳ���
                Collider[] objectsAbove = Physics.OverlapBox(transform.position, new Vector3(5f, 10f, 5f), Quaternion.identity);
                foreach (Collider obj in objectsAbove)
                {
                    Rigidbody rb = obj.gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // ���� ƨ�ܳ��� ���� �߰�
                        rb.AddForce(Vector3.up * height + Vector3.up * bounceForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
