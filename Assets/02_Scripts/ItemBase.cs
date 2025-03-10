using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBase : Interactable
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI ItemDescriptionText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Transform parentRarityParticles;
    private ParticleSystem myRarityParticle;
    [SerializeField] private ItemSO itemSO;
    public ItemSO ItemSO => itemSO;


    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        myRarityParticle.gameObject.SetActive(false);
        myRarityParticle = null;
    }

    private void Init()
    {
        int rarityIndex = (int)ItemSO.ItemRarity;

        if (rarityIndex < parentRarityParticles.childCount)
        {
            myRarityParticle = parentRarityParticles.GetChild(rarityIndex).GetComponent<ParticleSystem>();
        }

        myRarityParticle.gameObject.SetActive(true);
        myRarityParticle.Play();

        itemNameText.text = ItemSO.ItemName;
        ItemDescriptionText.text = ItemSO.Description;
        itemImage.sprite = ItemSO.Icon;
    }


    public override void SetInteract()
    {
        //상호작용할 함수를 이벤트에 등록하기 GetItem();

    }

    private void GetItem()
    {
        base.SetInteract();
        gameObject.SetActive(false);
    }

    public ItemSO GetItemSO()
    {
        return ItemSO;
    }
}
