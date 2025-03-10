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
        GameStart();
    }

    public void GameStart()
    {
    }


    public void SetGameReferences(GameReferences gameReferences)
    {
        this.GameReferences = gameReferences;
    }
}
