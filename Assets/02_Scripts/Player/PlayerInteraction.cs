using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Transform cameraTransform;
    private LayerMask interactableLayer; 
    private float interactDistance = 6f; 
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
            RaycastHit[] hits = Physics.RaycastAll(cameraTransform.position, cameraTransform.forward, interactDistance, interactableLayer);
            bool hitDetected = false; // 첫 번째 박스 콜라이더만 처리하도록 하는 변수

            foreach (var hit in hits)
            {
                // 박스 콜라이더일 경우
                if (hit.collider is BoxCollider)
                {
                    // 첫 번째 박스 콜라이더만 처리
                    if (!hitDetected)
                    {
                        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactDistance, Color.red);

                        Interactable interactable = hit.collider.GetComponent<Interactable>();

                        if (interactable != null)
                        {
                            if (currentInteractable != interactable)
                            {
                                if (currentInteractable != null)
                                {
                                    currentInteractable.ToggleActiveInfoUI(false);
                                }

                                currentInteractable = interactable;
                                currentInteractable.ToggleActiveInfoUI(true);
                            }
                        }

                        hitDetected = true; // 첫 번째 박스 콜라이더를 처리했으므로 더 이상 처리하지 않음
                        break; // 첫 번째 박스 콜라이더만 찾고 루프 종료
                    }
                }
            }

            // 충돌된 박스 콜라이더가 없을 경우
            if (!hitDetected)
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactDistance, Color.green);

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
