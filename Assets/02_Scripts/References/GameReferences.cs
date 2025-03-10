using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameReferences : MonoBehaviour
{
    [SerializeField] private GameObject inventoryVirtualCam;
    public GameObject InventoryVirtualCam { get; private set; }

    [SerializeField] private LayerMask interactableLayer;
    public LayerMask InteractableLayer { get; private set; }

    [SerializeField] private LayerMask groundLayer;
    public LayerMask GroundLayer { get; private set; }


    private void Awake()
    {
        InventoryVirtualCam = inventoryVirtualCam;
        InteractableLayer = interactableLayer;
        GroundLayer = groundLayer;

        GameManager manager = GameObject.FindObjectOfType<GameManager>();
        manager.SetGameReferences(this);
    }
}
