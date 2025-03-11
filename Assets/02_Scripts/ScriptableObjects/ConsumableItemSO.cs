using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Object/Item/Consumable", order = int.MaxValue)]
public class ConsumableItemSO : ItemSO
{
    [SerializeField] private CONSUMABLE_TYPE consumableType;
    public CONSUMABLE_TYPE ConsumableType => consumableType;

    private float duration = 10f;
    Player player;

    public void Use()
    {
        player = PlayerManager.Instance.GetPlayerReferences().Player;
        switch (ConsumableType)
        {
            case CONSUMABLE_TYPE.SpeedPotion:
                GameManager.Instance.GetComponent<MonoBehaviour>().StartCoroutine(SpeedBoostCoroutine());
                break;

            case CONSUMABLE_TYPE.Meat:
                GameManager.Instance.GetComponent<MonoBehaviour>().StartCoroutine(DrainHealthOverTime());
                break;
        }

        InventoryManager.Instance.RemoveItem(this);
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        player.maxSpeed += Value; // 이동 속도 증가
        yield return new WaitForSeconds(duration);
        player.maxSpeed -= Value; // 원래 속도로 복구
    }

    private IEnumerator DrainHealthOverTime()
    {
        float elapsedTime = 0f;
        float interval = 1f;
        int healthPerTick = Value / (int)(duration / interval);

        while (elapsedTime < duration)
        {
            player.TakeDamage(healthPerTick);

            elapsedTime += interval;
            yield return new WaitForSeconds(interval);
        }
    }
}
