using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReferences : MonoBehaviour
{
    [Header("Health and Stamina UI")]
    [SerializeField] private Image healthBarImage;
    public Image HealthBarImage { get; private set; }

    [SerializeField] private Image staminaBarImage;
    public Image StaminaBarImage { get; private set; }

    [SerializeField] private GameObject staminaBarObject;
    public GameObject StaminaBarObject { get; private set; }



    [Header("Inventory UI")]
    [SerializeField] private InventoryUI inventoryUI;
    public InventoryUI InventoryUI { get; private set; }

    [SerializeField] private Button inventoryButton;
    public Button InventoryButton { get; private set; }

    [SerializeField] private GameObject inventoryUIObject;
    public GameObject InventoryUIObject { get; private set; }



    [Header("Equipment UI")]
    //   [SerializeField] private EquipmentUI EquipmentUI;
    //   public EquipmentUI EquipmentUI { get; private set; }

    [SerializeField] private Button equipmentButton;
    public Button EquipmentButton { get; private set; }

    [SerializeField] private GameObject equipmentUIObject;
    public GameObject EquipmentUIObject { get; private set; }



    [Header("Etc UI")]

    [SerializeField] private GameObject menuCanvas;
    public GameObject MenuCanvas { get; private set; }

    [SerializeField] private GameObject hudCanvas;
    public GameObject HUDCanvas { get; private set; }


    [Header("Standby UI")]

    [SerializeField] private GameObject standbyCanvas;
    public GameObject StandbyCanvas { get; private set; }

    [SerializeField] private Button gameStartButton;
    public Button GameStartButton { get; private set; }

    [SerializeField] private Button gameExitButton;
    public Button GameExitButton { get; private set; }


    private void Awake()
    {
        HealthBarImage = healthBarImage;
        StaminaBarImage = staminaBarImage;
        StaminaBarObject = staminaBarObject;

        InventoryUI = inventoryUI;
        InventoryButton = inventoryButton;
        InventoryUIObject = inventoryUIObject;

        // EquipmentUI = EquipmentUI;
        EquipmentButton = equipmentButton;
        EquipmentUIObject = equipmentUIObject;

        MenuCanvas = menuCanvas;
        HUDCanvas = hudCanvas;

        StandbyCanvas = standbyCanvas;
        GameStartButton = gameStartButton;
        GameExitButton = gameExitButton;

        UIManager manager = GameObject.FindObjectOfType<UIManager>();
        manager.SetUIReferences(this);
    }
}
