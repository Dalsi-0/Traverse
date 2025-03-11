using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private UIReferences UIReferences;
    public UIReferences GetUIReferences() => UIReferences;


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

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        GetUIReferences().HUDCanvas.SetActive(false);
        PlayerManager.Instance.OnStaminaChanged += UpdateStaminaUI;
        PlayerManager.Instance.OnHpHeal += UpdateHpUI;
        PlayerManager.Instance.OnHpDamage += UpdateHpUI;
        GetUIReferences().MenuCanvas.SetActive(false);
        SetButtonListener();
    }

    private void UpdateStaminaUI()
    {
        GetUIReferences().StaminaBarObject.SetActive(PlayerManager.Instance.GetPlayerReferences().Player.stamina
            < PlayerManager.Instance.GetPlayerReferences().Player.maxStamina);

        GetUIReferences().StaminaBarImage.fillAmount = PlayerManager.Instance.GetPlayerReferences().Player.stamina
            / PlayerManager.Instance.GetPlayerReferences().Player.maxStamina;
    }

    private void UpdateHpUI()
    {
        GetUIReferences().HealthBarImage.fillAmount = PlayerManager.Instance.GetPlayerReferences().Player.hp
            / PlayerManager.Instance.GetPlayerReferences().Player.maxHp;
    }

    public void ToggleInventoryUI()
    {
        GetUIReferences().InventoryUIObject.SetActive(true);
        GetUIReferences().EquipmentUIObject.SetActive(false);
    }

    public void SetInventoryDetailUI(ItemSO itemSO, int amount)
    {
        if (itemSO.ItemType == ITEM_TYPE.Consumable)
        {
            GetUIReferences().InventorySelectItemUseButton.gameObject.SetActive(true);
        }
        else
        {
            GetUIReferences().InventorySelectItemUseButton.gameObject.SetActive(false);
        }

        GetUIReferences().InventorySelectItemImage.gameObject.SetActive(true);
        GetUIReferences().InventorySelectItemImage.sprite = itemSO.Icon;
        GetUIReferences().InventorySelectItemName.text = itemSO.ItemName;
        GetUIReferences().InventorySelectItemDes.text = itemSO.Description;
        GetUIReferences().InventorySelectItemStack.text = amount.ToString();
    }
    public void ResetInventoryDetailUI()
    {
        UIManager.Instance.GetUIReferences().InventorySelectItemUseButton.gameObject.SetActive(false);
        GetUIReferences().InventorySelectItemImage.gameObject.SetActive(false);
        GetUIReferences().InventorySelectItemImage.sprite = GetUIReferences().InventorySelectItemNULLImage;
        GetUIReferences().InventorySelectItemName.text = "";
        GetUIReferences().InventorySelectItemDes.text = "";
        GetUIReferences().InventorySelectItemStack.text = "";
    }

    public void ToggleEquipmentUI()
    {
        GetUIReferences().EquipmentUIObject.SetActive(true);
        GetUIReferences().InventoryUIObject.SetActive(false);
    }

    private void EndStandby()
    {
        GetUIReferences().StandbyCanvas.SetActive(false);
        GameManager.Instance.GameStart();
    }
    private void EndGame()
    {
        Application.Quit();
    }

    private void SetButtonListener()
    {
        GetUIReferences().GameStartButton.onClick.AddListener(EndStandby);
        GetUIReferences().GameExitButton.onClick.AddListener(EndGame);
        GetUIReferences().InventoryButton.onClick.AddListener(ToggleInventoryUI);
        GetUIReferences().EquipmentButton.onClick.AddListener(ToggleEquipmentUI);
    }
    public void SetUIReferences(UIReferences uiReferences)
    {
        this.UIReferences = uiReferences;
    }
}
