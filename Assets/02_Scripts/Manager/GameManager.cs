using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameReferences GameReferences;
    public GameReferences GetUIReferences() => GameReferences;

    private InputAction menuAction;

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

#if UNITY_EDITOR
    private void OnValidate()
    {
        menuAction = new InputAction("Menu", binding: "<Keyboard>/tab");
    }
#endif

    private void OnEnable()
    {
        menuAction.started += ToggleMenuInput;
        menuAction.Enable();
    }

    private void OnDisable()
    {
        menuAction.started -= ToggleMenuInput;
        menuAction.Disable(); 
    }
    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        LockCursor();
    }

    private void ToggleMenuInput(InputAction.CallbackContext context)
    {
        bool isMenuActive = UIManager.Instance.GetUIReferences().MenuCanvas.activeSelf;

        UIManager.Instance.GetUIReferences().MenuCanvas.SetActive(!isMenuActive);
        GetUIReferences().InventoryVirtualCam.SetActive(!isMenuActive);
       
        if (isMenuActive)
        {
            LockCursor();
            StartCoroutine(WaitForInputToResume());
        }
        else
        {
            UnlockCursor();
            PlayerManager.Instance.GetPlayerReferences().PlayerInput.LockInput();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator WaitForInputToResume()
    {
        yield return new WaitForSeconds(0.6f);
        PlayerManager.Instance.GetPlayerReferences().PlayerInput.UnlockInput();
    }

    public void SetGameReferences(GameReferences gameReferences)
    {
        this.GameReferences = gameReferences;
    }
}
