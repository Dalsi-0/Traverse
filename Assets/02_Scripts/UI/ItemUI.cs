using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemStackText;
    [SerializeField] private Image itemIcon;
    private ItemSO itemSO;

    public void SetItemData(ItemSO item, int amount)
    {
        itemSO = item;
        if (amount > 1)
        {
            itemStackText.text = amount.ToString();
        }
        else
        {
            itemStackText.text = "";
        }
        itemIcon.sprite = itemSO.Icon;
        itemIcon.SetNativeSize();
    }
}
