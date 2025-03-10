using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    // 아이템 저장 공간
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
        if (item.StackCount > 1) // 스택 가능한 아이템이라면
        {
            if (dicInventory.ContainsKey(item))
            {
                int currentAmount = dicInventory[item];
                if (currentAmount + amount <= item.StackCount) // 스택 내에서 추가 가능
                {
                    dicInventory[item] += amount;
                }
                else
                {
                    int remainingAmount = amount - (item.StackCount - currentAmount); // 넘치는 아이템 수
                    dicInventory[item] = item.StackCount; // 최대 스택 수로 설정

                    // 남은 아이템은 새로운 슬롯에 추가
                    AddItem(item, remainingAmount);
                }
            }
            else
            {
                dicInventory[item] = amount;
            }
        }
        else // 스택 불가능한 아이템이라면 
        {
            if (dicInventory.ContainsKey(item))
            {
                dicInventory[item] += amount; // 그냥 추가
            }
            else
            {
                dicInventory[item] = amount;
            }
        }

        // UI 업데이트 호출
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

        // UI 업데이트 호출
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
