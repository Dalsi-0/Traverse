using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    private Vector2 dir;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction leftClickAction;
    private InputAction rightClickAction;
    private InputAction interactAction;
    private InputAction menuAction;

    public UnityAction jumpEvent;
    public UnityAction leftClickStartedEvent;
    public UnityAction leftClickCanceledEvent;
    public UnityAction rightClickStartedEvent;
    public UnityAction rightClickCanceledEvent;
    public UnityAction interactEvent;
    public UnityAction menuEvent;

    public Vector2 GetMoveDirection() => dir;

    private void Awake()
    {
        moveAction = new InputAction("Move", type: InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        jumpAction = new InputAction("Jump", binding: "<Keyboard>/space");
        leftClickAction = new InputAction("LeftClick", binding: "<Mouse>/leftButton");
        rightClickAction = new InputAction("RightClick", binding: "<Mouse>/rightButton");
        interactAction = new InputAction("Interact", binding: "<Keyboard>/f");
        menuAction = new InputAction("Menu", binding: "<Keyboard>/tab");


        moveAction.performed += PlayerMove;
        moveAction.canceled += PlayerStop;
        jumpAction.started += PlayerJump;
        leftClickAction.started += PlayerLeftClickStarted;
        leftClickAction.canceled += PlayerLeftClickCanceled;
        rightClickAction.started += PlayerRightClickStarted;
        rightClickAction.canceled += PlayerRightClickCanceled;
        
        interactAction.started += PlayerInteract;
        menuAction.started += ToggleMenuInput;

        LockCursor();
    }


    private void OnEnable()
    {
        jumpAction.Enable();
        moveAction.Enable();
        leftClickAction.Enable();
        rightClickAction.Enable();
        interactAction.Enable();
        menuAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
        moveAction.Disable();
        leftClickAction.Disable();
        rightClickAction.Disable();
        interactAction.Disable();
        menuAction.Disable();
    }

    void PlayerJump(InputAction.CallbackContext value)
    {
        jumpEvent?.Invoke(); 
    }

    void PlayerMove(InputAction.CallbackContext value)
    {
        dir = value.ReadValue<Vector2>(); 
    }

    void PlayerStop(InputAction.CallbackContext value)
    {
        dir = Vector2.zero;
    }

    void PlayerLeftClickStarted(InputAction.CallbackContext value)
    {
        leftClickStartedEvent?.Invoke();
    }

    void PlayerLeftClickCanceled(InputAction.CallbackContext value)
    {
        leftClickCanceledEvent?.Invoke(); 
    }

    void PlayerRightClickStarted(InputAction.CallbackContext value)
    { 
        rightClickStartedEvent?.Invoke();
    }

    void PlayerRightClickCanceled(InputAction.CallbackContext value)
    {
        rightClickCanceledEvent?.Invoke();
    }

    void PlayerInteract(InputAction.CallbackContext value)
    {
        interactEvent?.Invoke();
    }


    public void LockInput()
    {
        moveAction.Disable();
        jumpAction.Disable();
        leftClickAction.Disable();
        interactAction.Disable();
        rightClickAction.Disable();
    }

    public void UnlockInput()
    {
        moveAction.Enable();
        jumpAction.Enable();
        leftClickAction.Enable();
        interactAction.Enable();
        rightClickAction.Enable();
    }


    private void ToggleMenuInput(InputAction.CallbackContext context)
    {
        bool isMenuActive = UIManager.Instance.GetUIReferences().MenuCanvas.activeSelf;

        UIManager.Instance.GetUIReferences().MenuCanvas.SetActive(!isMenuActive);
        GameManager.Instance.GetGameReferences().InventoryVirtualCam.SetActive(!isMenuActive);

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
}
