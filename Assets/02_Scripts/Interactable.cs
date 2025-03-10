using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Transform cameraTransform; 
    [SerializeField] private GameObject worldCanvas; 
    [SerializeField] private GameObject infoUI;
    [SerializeField] private Collider infoCollider; 
    private Coroutine lookAtCoroutine;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (worldCanvas != null)
        {
            worldCanvas.SetActive(false);
        }

        if (infoUI != null)
        {
            infoUI.SetActive(false);
        }

        if (infoCollider != null)
        {
            infoCollider.enabled = false;
        }
    }

    public virtual void SetInteract()
    {
        infoUI.SetActive(true);
    }

    private IEnumerator LookAtTarget(Transform target)
    {
        while (true)
        {
            Vector3 direction = cameraTransform.position - transform.position; 
            direction.y = 0; 

            transform.rotation = Quaternion.LookRotation(direction);
            yield return null; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (lookAtCoroutine == null)
            {
                lookAtCoroutine = StartCoroutine(LookAtTarget(other.transform));
            }

            if (worldCanvas != null && !worldCanvas.activeSelf)
            {
                worldCanvas.SetActive(true);
            }

            if (infoCollider != null && !infoCollider.enabled)
            {
                infoCollider.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (lookAtCoroutine != null)
            {
                StopCoroutine(lookAtCoroutine);
                lookAtCoroutine = null;
            }

            if (worldCanvas != null)
            {
                worldCanvas.SetActive(false);
            }

            if (infoUI != null)
            {
                infoUI.SetActive(false);
            }

            if (infoCollider != null)
            {
                infoCollider.enabled = false;
            }
        }
    }
}
