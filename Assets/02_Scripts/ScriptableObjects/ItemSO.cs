using UnityEngine;

public enum ITEM_TYPE
{
    Material,  // ㄴㄷㄹㄴㄷㄹㄴㄷㄹ
    Equipment, // ㄴㄷㄹㄴㄷㄹㄴㄷㄹ
    Consumable // ㄴㄷㄹㄴㄷㄹㄴㄷㄹ
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class ItemSO : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private ITEM_TYPE itemType;
    [SerializeField] private Sprite icon;
    [SerializeField] private int value;

    [TextArea]
    [SerializeField] private string description; 

    public string ItemName => itemName;
    public ITEM_TYPE ItemType => itemType;
    public Sprite Icon => icon;
    public int Value => value;
    public string Description => description;
}