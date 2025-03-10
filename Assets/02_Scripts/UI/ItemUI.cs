using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemStackText;
    [SerializeField] private Image itemIcon;
    private ItemSO itemSO;

    public void SetItemData(ItemSO item)
    {
        itemSO = item;
        itemIcon.sprite = itemSO.Icon;
        itemIcon.SetNativeSize();
    }
}
