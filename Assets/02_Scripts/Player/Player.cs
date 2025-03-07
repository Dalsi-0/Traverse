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

    public float staminaRegenRate = 5f;
    public float staminaRegenDelay = 2f;
    private Coroutine staminaRegenCoroutine;

    // 체력 감소
    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0f)
        {
            Die();
        }
    }

    // 체력 회복
    public void Heal(float amount)
    {
        hp += amount;
        if (hp > maxHp) hp = maxHp;
    }

    // 스태미나 소비
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

    // 스태미나 회복 코루틴
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

    // 공격 메서드
    public void Attack()
    {
        // 공격 로직 (예시: 공격력에 따른 데미지 계산)
        Debug.Log("Attacking with " + attackPower + " damage.");
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // 플레이어 사망 처리
    }
}
