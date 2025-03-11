using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemUIPrefab; 
    [SerializeField] private Transform content; 

    [SerializeField] private TextMeshProUGUI itemNameText; 
    [SerializeField] private TextMeshProUGUI itemDesText; 
    [SerializeField] private TextMeshProUGUI itemStackText;
    [SerializeField] private Image itemImage; 

    private void Start()
    {
        UpdateInventoryUI();
    }

    // 아이템을 추가하거나 제거할 때마다 호출되는 함수
    public void UpdateInventoryUI()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in InventoryManager.Instance.GetInventoryItems())
        {
            CreateItemUI(item.item, item.count);
        }
    }

    // 아이템 UI를 생성하고 설정하는 함수
    private void CreateItemUI(ItemSO item, int amount)
    {
        ItemUI itemUI = Instantiate(itemUIPrefab, content).transform.GetComponent<ItemUI>();
        itemUI.SetItemData(item, amount);

        if (amount > 1)
        {
            itemStackText.text = amount.ToString() + "개";
        }
        else
        {
            itemStackText.text = "";
        }
    }
}
