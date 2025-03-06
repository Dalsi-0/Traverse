using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDUI : MonoBehaviour
{
    public Image healthBarImage;
    public Image staminaBarImage;

    public void UpdateHealthBar(float healthRatio)
    {
        healthBarImage.fillAmount = Mathf.Clamp01(healthRatio);
    }

    public void UpdateStaminaBar(float staminaRatio)
    {
        staminaBarImage.fillAmount = Mathf.Clamp01(staminaRatio);
    }
}
