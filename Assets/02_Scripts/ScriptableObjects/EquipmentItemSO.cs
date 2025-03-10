using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Object/Item/Equipment", order = int.MaxValue)]
public class EquipmentItemSO : ItemSO
{
    [SerializeField] private EQUIPMENT_TYPE equipmentType;
    public EQUIPMENT_TYPE EquipmentType => equipmentType;
}
