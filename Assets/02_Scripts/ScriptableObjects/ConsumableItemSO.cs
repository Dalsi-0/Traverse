using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Object/Item/Consumable", order = int.MaxValue)]
public class ConsumableItemSO : ItemSO
{
    [SerializeField] private CONSUMABLE_TYPE consumableType;
    public CONSUMABLE_TYPE ConsumableType => consumableType;
}
