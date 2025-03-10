using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : Interactable
{
    public float moveSpeed = 2f; 
    private bool interact = false;
    private bool isMoving = false; 
    private Vector3 startPosition;

    public override void Interact()
    {
        if (!isMoving)
        {
            interact = true;
            startPosition = transform.position; 
            StartCoroutine(MovePlatform());
        }
    }

    private IEnumerator MovePlatform()
    {
        isMoving = true;

        float timeElapsed = 0f;
        float moveDuration = 2f; 

        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y + 50f, startPosition.z);

        while (timeElapsed < moveDuration)
        {
            float step = moveSpeed * Time.fixedDeltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            timeElapsed += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate(); 
        }
        PlayerManager.Instance.GetPlayerReferences().PlayerController.gameObject.transform.SetParent(null);

        if (PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.isCheckingRaycast)
        {
            PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.isCheckingRaycast = false;
            PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.StopInteractCheck();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interact)
        {
            other.transform.SetParent(transform); // 플레이어를 발판의 자식으로 설정
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            interact = false;
            other.transform.SetParent(null); // 플레이어를 다시 원래 상태로 복귀
        }
    }
}
