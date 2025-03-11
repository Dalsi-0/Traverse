using UnityEngine;
using System.Collections;
using static IEntityActions;

public class Player : MonoBehaviour, IDamageable
{
    public float hp = 100f;
    public float stamina = 100f;
    public float attackPower = 10f;
    public float maxHp = 100f;
    public float maxStamina = 100f;
    public float maxSpeed = 10f;

    public float staminaRegenRate = 5f;
    public float staminaRegenDelay = 2f;
    private Coroutine staminaRegenCoroutine;

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0f)
        {
            Die();
        }
        PlayerManager.Instance.NotifyHpDamage();
    }

    public void Heal(float amount)
    {
        hp += amount;
        if (hp > maxHp) hp = maxHp;
        PlayerManager.Instance.NotifyHpHeal();
    }

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

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(staminaRegenDelay);

        while (stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;

            PlayerManager.Instance.NotifyStaminaChanged();
            if (stamina >= maxStamina)
            {
                stamina = maxStamina;
                staminaRegenCoroutine = null;
                yield break;
            }
            yield return null;
        }
    }

    public void Attack()
    {
        Debug.Log(attackPower + " damage.");
    }

    private void Die()
    {
        Debug.Log("Player die.");
    }
}
