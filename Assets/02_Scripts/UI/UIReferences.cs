using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReferences : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    public Image HealthBarImage { get; private set; }

    [SerializeField] private Image staminaBarImage;
    public Image StaminaBarImage { get; private set; }

    [SerializeField] private GameObject staminaBarObject;
    public GameObject StaminaBarObject { get; private set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        HealthBarImage = healthBarImage;
        StaminaBarImage = staminaBarImage;
        StaminaBarObject = staminaBarObject;

        UIManager manager = GameObject.FindObjectOfType<UIManager>();
        manager.SetUIReferences(this);
    }
#endif
}
