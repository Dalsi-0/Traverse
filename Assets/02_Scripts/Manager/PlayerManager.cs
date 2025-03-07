using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    public event Action OnHpHeal;
    public event Action OnHpDamage;
    public event Action OnStaminaChanged;

    private PlayerReferences playerReferences;
    public PlayerReferences GetPlayerReferences() => playerReferences;

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

    public void NotifyHpHeal()
    {
        OnHpHeal?.Invoke();
    }

    public void NotifyHpDamage()
    {
        OnHpDamage?.Invoke();
    }

    public void NotifyStaminaChanged()
    {
        OnStaminaChanged?.Invoke();
    }

    public void SetPlayerReferences(PlayerReferences playerReferences)
    {
        this.playerReferences = playerReferences;
    }
}
