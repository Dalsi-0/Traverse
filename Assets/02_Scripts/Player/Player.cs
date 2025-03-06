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
    // 체력 감소
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    // 체력 회복
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }

    // 스태미나 소비
    public void ConsumeStamina(float amount)
    {
        stamina -= amount;
        if (stamina < 0f) stamina = 0f;
    }

    // 스태미나 회복
    public void RegenerateStamina(float amount)
    {
        stamina += amount;
        if (stamina > maxStamina) stamina = maxStamina;
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
