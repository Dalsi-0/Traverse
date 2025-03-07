using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    private Vector2 dir;
    private bool isLeftClickHeld = false;
    private bool isRightClickHeld = false;

    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction leftClickAction;
    public InputAction rightClickAction;
    public InputAction interactAction;

    public UnityAction jumpEvent;
    public UnityAction leftClickStartedEvent;
    public UnityAction leftClickCanceledEvent;
    public UnityAction rightClickStartedEvent;
    public UnityAction rightClickCanceledEvent;
    public UnityAction interactEvent;

    public Vector2 GetMoveDirection() => dir;
    public bool IsLeftClickHeld() => isLeftClickHeld;
    public bool IsRightClickHeld() => isRightClickHeld;

    private void OnValidate()
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

        moveAction.performed += PlayerMove;
        moveAction.canceled += PlayerStop;
        jumpAction.started += PlayerJump;
        leftClickAction.started += PlayerLeftClickStarted;
        leftClickAction.canceled += PlayerLeftClickCanceled;
        rightClickAction.started += PlayerRightClickStarted;
        rightClickAction.canceled += PlayerRightClickCanceled;
        interactAction.started += PlayerInteract;
    }

    private void OnEnable()
    {
        jumpAction.Enable();
        moveAction.Enable();
        leftClickAction.Enable();
        rightClickAction.Enable();
        interactAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
        moveAction.Disable();
        leftClickAction.Disable();
        rightClickAction.Disable();
        interactAction.Disable();
    }

    // 점프 동작 처리
    void PlayerJump(InputAction.CallbackContext value)
    {
        jumpEvent?.Invoke(); 
    }

    // 이동 처리
    void PlayerMove(InputAction.CallbackContext value)
    {
        dir = value.ReadValue<Vector2>(); 
    }

    // 이동 취소 처리
    void PlayerStop(InputAction.CallbackContext value)
    {
        dir = Vector2.zero;
    }

    // 좌클릭 시작 시 처리
    void PlayerLeftClickStarted(InputAction.CallbackContext value)
    {
        isLeftClickHeld = true; 
        leftClickStartedEvent?.Invoke();
    }

    // 좌클릭 해제 시 처리
    void PlayerLeftClickCanceled(InputAction.CallbackContext value)
    {
        isLeftClickHeld = false; 
        leftClickCanceledEvent?.Invoke(); 
    }

    // 우클릭 시작 시 처리
    void PlayerRightClickStarted(InputAction.CallbackContext value)
    {
        isRightClickHeld = true; 
        rightClickStartedEvent?.Invoke();
    }

    // 우클릭 해제 시 처리
    void PlayerRightClickCanceled(InputAction.CallbackContext value)
    {
        isRightClickHeld = false; 
        rightClickCanceledEvent?.Invoke();
    }

    // 상호작용 처리
    void PlayerInteract(InputAction.CallbackContext value)
    {
        interactEvent?.Invoke(); 
    }
}
