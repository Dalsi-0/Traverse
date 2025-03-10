using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemUIPrefab; // 아이템 UI 프리팹
    [SerializeField] private Transform content; // 인벤토리 아이템들이 배치될 Content 

    // 아이템 설명창
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
        // 기존에 생성된 모든 아이템 UI 삭제
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // InventoryManager의 아이템을 가져와서 UI를 생성
        foreach (var item in InventoryManager.Instance.GetInventoryItems())
        {
            CreateItemUI(item.Key, item.Value);
        }
    }

    // 아이템 UI를 생성하고 설정하는 함수
    private void CreateItemUI(ItemSO item, int amount)
    {
        GameObject itemUI = Instantiate(itemUIPrefab, content);

      // itemNameText = itemUI.transform.GetChild(1).GetComponent<Text>(); // 아이템 이름
      // itemDesText = itemUI.transform.GetChild(1).GetComponent<Text>(); // 아이템 이름
      // itemImage = itemUI.transform.GetChild(0).GetComponent<Image>(); // 아이콘 이미지
      // itemStackText = itemUI.transform.GetChild(2).GetComponent<Text>(); // 스택 수

        // 스택 수 표시 (스택이 1 이상일 때만 표시)
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
