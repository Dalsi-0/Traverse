using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReferences : MonoBehaviour
{
    [SerializeField] private GameObject inventoryVirtualCam;
    public GameObject InventoryVirtualCam { get; private set; }


#if UNITY_EDITOR
    private void OnValidate()
    {
        InventoryVirtualCam = inventoryVirtualCam;

        GameManager manager = GameObject.FindObjectOfType<GameManager>();
        manager.SetGameReferences(this);
    }
#endif
}
