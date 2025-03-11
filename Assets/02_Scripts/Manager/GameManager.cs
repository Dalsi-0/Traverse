using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameReferences GameReferences;
    public GameReferences GetGameReferences() => GameReferences;


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
        PlayerManager.Instance.GetPlayerReferences().PlayerController.PlayerRigidbody.isKinematic = true;
        GetGameReferences().StandbyVirtualCam.SetActive(true);
    }


    public void GameStart()
    {
        GetGameReferences().StandbyVirtualCam.SetActive(false);
        UIManager.Instance.GetUIReferences().HUDCanvas.SetActive(true);
        PlayerManager.Instance.GetPlayerReferences().PlayerController.PlayerRigidbody.isKinematic = false;
        PlayerManager.Instance.GetPlayerReferences().PlayerController.EndStandby();
        PlayerManager.Instance.GetPlayerReferences().PlayerInput.LockCursor();
    }


    public void SetGameReferences(GameReferences gameReferences)
    {
        this.GameReferences = gameReferences;
    }
}
