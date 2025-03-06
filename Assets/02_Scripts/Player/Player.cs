using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100f;
    public float stamina = 100f;
    public float attackPower = 10f;
    public float maxHealth = 100f;
    public float maxStamina = 100f;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    // ü�� ����
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    // ü�� ȸ��
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }

    // ���¹̳� �Һ�
    public void ConsumeStamina(float amount)
    {
        stamina -= amount;
        if (stamina < 0f) stamina = 0f;
    }

    // ���¹̳� ȸ��
    public void RegenerateStamina(float amount)
    {
        stamina += amount;
        if (stamina > maxStamina) stamina = maxStamina;
    }


    // ���� �޼���
    public void Attack()
    {
        // ���� ���� (����: ���ݷ¿� ���� ������ ���)
        Debug.Log("Attacking with " + attackPower + " damage.");
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // �÷��̾� ��� ó��
    }
}
