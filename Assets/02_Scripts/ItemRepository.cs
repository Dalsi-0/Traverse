using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class ItemRepository : MonoBehaviour
{
    private Dictionary<MATERIAL_TYPE, ItemSO> dicMaterialItemSO = new Dictionary<MATERIAL_TYPE, ItemSO>();
    private Dictionary<MATERIAL_TYPE, GameObject> dicMaterialItemPrefabs = new Dictionary<MATERIAL_TYPE, GameObject>();

    private Dictionary<EQUIPMENT_TYPE, ItemSO> dicEquipmentItemSO = new Dictionary<EQUIPMENT_TYPE, ItemSO>();
    private Dictionary<EQUIPMENT_TYPE, GameObject> dicEquipmentItemPrefabs = new Dictionary<EQUIPMENT_TYPE, GameObject>();

    private Dictionary<CONSUMABLE_TYPE, ItemSO> dicConsumableItemSO = new Dictionary<CONSUMABLE_TYPE, ItemSO>();
    private Dictionary<CONSUMABLE_TYPE, GameObject> dicConsumableItemPrefabs = new Dictionary<CONSUMABLE_TYPE, GameObject>();


    [SerializeField] private ItemSO[] ItemSOs;
    [SerializeField] private GameObject[] ItemPrefabs;

    private void Awake()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        dicMaterialItemSO.Clear();
        dicMaterialItemPrefabs.Clear();
        dicEquipmentItemSO.Clear();
        dicEquipmentItemPrefabs.Clear();
        dicConsumableItemSO.Clear();
        dicConsumableItemPrefabs.Clear();

        for (int i = 0; i < ItemSOs.Length; i++)
        {
            switch (ItemSOs[i].ItemType)
            {
                case ITEM_TYPE.Material:
                    MaterialItemSO materialItem = (MaterialItemSO)ItemSOs[i];
                    dicMaterialItemSO[materialItem.MaterialType] = materialItem;
                    dicMaterialItemPrefabs[materialItem.MaterialType] = FindPrefab(materialItem);
                    break;

                case ITEM_TYPE.Equipment:
                    EquipmentItemSO equipmentItem = (EquipmentItemSO)ItemSOs[i];
                    dicEquipmentItemSO[equipmentItem.EquipmentType] = equipmentItem;
                    dicEquipmentItemPrefabs[equipmentItem.EquipmentType] = FindPrefab(equipmentItem);
                    break;

                case ITEM_TYPE.Consumable:
                    ConsumableItemSO consumableItem = (ConsumableItemSO)ItemSOs[i];
                    dicConsumableItemSO[consumableItem.ConsumableType] = consumableItem;
                    dicConsumableItemPrefabs[consumableItem.ConsumableType] = FindPrefab(consumableItem);
                    break;
            }
        }
    }

    private GameObject FindPrefab(ItemSO item)
    {
        for (int j = 0; j < ItemPrefabs.Length; j++)
        {
            if (ItemPrefabs[j].GetComponent<ItemBase>().GetItemSO() == item)
            {
                return ItemPrefabs[j];
            }
        }
        return null;
    }
}
