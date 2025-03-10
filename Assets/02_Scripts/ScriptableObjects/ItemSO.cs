using UnityEngine;


public class ItemSO : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private ITEM_TYPE itemType;
    [SerializeField] private ITEM_RARITY itemRarity;
    [SerializeField] private Sprite icon;
    [SerializeField] private int value;
    [TextArea]
    [SerializeField] private string description; 

    public string ItemName => itemName;
    public ITEM_TYPE ItemType => itemType;
    public ITEM_RARITY ItemRarity => itemRarity;
    public Sprite Icon => icon;
    public int Value => value;
    public string Description => description;
}