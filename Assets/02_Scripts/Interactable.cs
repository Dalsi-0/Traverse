﻿using System.Collections;
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


    public void ToggleActiveInfoUI(bool isActive)
    {
        infoUI.SetActive(isActive);
    }

    public virtual void Interact()
    {

    }

    private IEnumerator LookAtTarget(Transform target)
    {
        while (true)
        {
            Vector3 direction = cameraTransform.position - worldCanvas.transform.position; 
            direction.y = 0;

            worldCanvas.transform.rotation = Quaternion.LookRotation(direction);
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

            if (!PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.isCheckingRaycast)
            {
                PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.isCheckingRaycast = true;
                PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.StartInteractCheck();
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
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

            if (PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.isCheckingRaycast)
            {
                PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.isCheckingRaycast = false;
                PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.StopInteractCheck();
            }
        }
    }
}
