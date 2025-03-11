using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : Interactable
{
    private bool interact = false;
    private Rigidbody rb;
    private float launchForce = 40f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public override void Interact()
    {
        if (!interact)
        {
            interact = true;
            LaunchPlatform();
        }
    }

    private void LaunchPlatform()
    {
        if (rb != null)
        {
            PlayerManager.Instance.GetPlayerReferences().PlayerController.gameObject.transform.SetParent(transform);

            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);


            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;

            StartCoroutine(DestroyAfterSeconds(3f)); 
        }
    }

    private IEnumerator DestroyAfterSeconds(float delay)
    {
        PlayerManager.Instance.GetPlayerReferences().PlayerController.gameObject.transform.SetParent(null);

        if (PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.isCheckingRaycast)
        {
            PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.isCheckingRaycast = false;
            PlayerManager.Instance.GetPlayerReferences().PlayerInteraction.StopInteractCheck();
        }

        yield return new WaitForSeconds(delay);

            
        Destroy(gameObject);
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            interact = false;
        }
    }
}
