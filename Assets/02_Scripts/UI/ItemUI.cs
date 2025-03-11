using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemStackText;
    [SerializeField] private Image itemIcon;
    private ItemSO itemSO;
    private int myAmount;

    public void SetItemData(ItemSO item, int amount)
    {
        itemSO = item;
        myAmount = amount;
        if (myAmount > 1)
        {
            itemStackText.text = myAmount.ToString();
        }
        else
        {
            itemStackText.text = "";
        }
        itemIcon.sprite = itemSO.Icon;
        itemIcon.SetNativeSize();
    }

    public void SelectThisItem()
    {
        UIManager.Instance.GetUIReferences().InventorySelectItemUseButton.onClick.RemoveAllListeners();
        UIManager.Instance.GetUIReferences().InventorySelectItemUseButton.onClick.AddListener(() => ((ConsumableItemSO)itemSO).Use());
        UIManager.Instance.SetInventoryDetailUI(itemSO, myAmount);
    }
}
