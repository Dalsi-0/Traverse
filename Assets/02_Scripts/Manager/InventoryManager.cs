using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InventorySlot
{
    public ItemSO item;  
    public int count;    

    public InventorySlot(ItemSO item, int count)
    {
        this.item = item;
        this.count = count;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<InventorySlot> inventorySlots = new List<InventorySlot>();

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
        while (amount > 0)
        {
            InventorySlot slot = inventorySlots.Find(s => s.item == item && s.count < item.StackCount);

            if (slot != null)
            {
                int spaceLeft = item.StackCount - slot.count;
                int addAmount = Mathf.Min(amount, spaceLeft);
                slot.count += addAmount;
                amount -= addAmount;
            }
            else
            {
                int addAmount = Mathf.Min(amount, item.StackCount);
                inventorySlots.Add(new InventorySlot(item, addAmount));
                amount -= addAmount;
            }
        }

        UIManager.Instance.GetUIReferences().InventoryUI.UpdateInventoryUI();
    }


    // 아이템 제거
    public bool RemoveItem(ItemSO item, int amount = 1)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item == item)
            {
                if (inventorySlots[i].count >= amount)
                {
                    inventorySlots[i].count -= amount;
                    if (inventorySlots[i].count == 0)
                    {
                        inventorySlots.RemoveAt(i);
                    }
                    UIManager.Instance.GetUIReferences().InventoryUI.UpdateInventoryUI();
                    return true;
                }
                else
                {
                    amount -= inventorySlots[i].count;
                    inventorySlots.RemoveAt(i);
                    i--;
                }
            }
        }
        return false;
    }


    // 인벤토리 슬롯 리스트 반환
    public List<InventorySlot> GetInventoryItems()
    {
        return inventorySlots;
    }
}
