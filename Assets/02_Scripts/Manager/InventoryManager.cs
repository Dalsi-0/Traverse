using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<ItemSO, int> dicInventory = new Dictionary<ItemSO, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 인벤토리에 아이템 추가
    public void AddItem(ItemSO item, int amount = 1)
    {
        if (item.StackCount > 1)
        {
            if (dicInventory.ContainsKey(item))
            {
                int currentAmount = dicInventory[item];
                if (currentAmount + amount <= item.StackCount)
                {
                    dicInventory[item] += amount;
                }
                else
                {
                    int remainingAmount = amount - (item.StackCount - currentAmount); 
                    dicInventory[item] = item.StackCount;

                    AddItem(item, remainingAmount);
                }
            }
            else
            {
                dicInventory[item] = amount;
            }
        }
        else 
        {
            if (dicInventory.ContainsKey(item))
            {
                dicInventory[item] += amount; 
            }
            else
            {
                dicInventory[item] = amount;
            }
        }

        UIManager.Instance.GetUIReferences().InventoryUI.UpdateInventoryUI();
    }


    // 아이템 제거
    public bool RemoveItem(ItemSO item, int amount = 1)
    {
        if (dicInventory.ContainsKey(item))
        {
            if (dicInventory[item] >= amount)
            {
                dicInventory[item] -= amount;

                if (dicInventory[item] <= 0)
                    dicInventory.Remove(item);

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        UIManager.Instance.GetUIReferences().InventoryUI.UpdateInventoryUI();
    }


    // 아이템 개수 확인
    public int GetItemCount(ItemSO item)
    {
        return dicInventory.ContainsKey(item) ? dicInventory[item] : 0;
    }

    public void PrintInventory()
    {
        Debug.Log("현재 인벤토리 목록:");
        foreach (var item in dicInventory)
        {
            Debug.Log($"{item.Key.ItemName}: {item.Value}개");
        }
    }

    // 인벤토리에 있는 아이템들을 가져오는 함수
    public Dictionary<ItemSO, int> GetInventoryItems()
    {
        return dicInventory;
    }

}
