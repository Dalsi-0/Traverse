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
    public void ToggleEquipmentUI()
    {
        GetUIReferences().EquipmentUIObject.SetActive(true);
        GetUIReferences().InventoryUIObject.SetActive(false);
    }


    private void SetButtonListener()
    {
        GetUIReferences().InventoryButton.onClick.AddListener(ToggleInventoryUI);
        GetUIReferences().EquipmentButton.onClick.AddListener(ToggleEquipmentUI);
    }
    public void SetUIReferences(UIReferences uiReferences)
    {
        this.UIReferences = uiReferences;
    }
}
