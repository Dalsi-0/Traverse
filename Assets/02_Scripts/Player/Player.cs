using UnityEngine;
using System.Collections;
using static IEntityActions;

public class Player : MonoBehaviour, IDamageable
{
    public float health = 100f;
    public float stamina = 100f;
    public float attackPower = 10f;
    public float maxHealth = 100f;
    public float maxStamina = 100f;

    public float staminaRegenRate = 5f;
    public float staminaRegenDelay = 2f;
    private Coroutine staminaRegenCoroutine;

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
    public bool ConsumeStamina(float amount)
    {
        if (stamina < amount)
        {
            return false;
        }

        stamina -= amount;

        PlayerManager.Instance.NotifyStaminaChanged();

        if (staminaRegenCoroutine != null)
        {
            StopCoroutine(staminaRegenCoroutine);
        }
        staminaRegenCoroutine = StartCoroutine(RegenerateStamina());

        return true;
    }

    // ���¹̳� ȸ�� �ڷ�ƾ
    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(staminaRegenDelay);

        while (stamina < maxStamina)
        {
            PlayerManager.Instance.NotifyStaminaChanged();
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;

                staminaRegenCoroutine = null;
                yield break;
            }
            yield return null;
        }
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
