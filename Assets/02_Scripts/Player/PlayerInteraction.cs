using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Transform cameraTransform;
    private LayerMask interactableLayer; 
    private float interactDistance = 3f; 
    private Interactable currentInteractable; // 현재 감지된 상호작용 오브젝트
    private Coroutine interactCheckRoutine;
    public bool isCheckingRaycast;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        interactableLayer = GameManager.Instance.GetGameReferences().InteractableLayer;
        PlayerManager.Instance.GetPlayerReferences().PlayerInput.interactEvent += HandleInteraction;
    }

    private IEnumerator CheckInteractable()
    {
        while (true)
        {
            RaycastHit hit; 
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, interactDistance, interactableLayer))
            {
                if (hit.collider is BoxCollider) 
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        if (currentInteractable != interactable)
                        {
                            currentInteractable.ToggleActiveInfoUI(false);
                            currentInteractable = interactable;
                            currentInteractable.ToggleActiveInfoUI(true);
                        }
                    }
                }
            }
            else
            {
                if (currentInteractable != null)
                {
                    currentInteractable.ToggleActiveInfoUI(false);
                    currentInteractable = null;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void HandleInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }


    public void StartInteractCheck()
    {
        interactCheckRoutine = StartCoroutine(CheckInteractable());
    }

    public void StopInteractCheck()
    {
        if (interactCheckRoutine != null)
        {
            StopCoroutine(interactCheckRoutine);
        }
    }
}
