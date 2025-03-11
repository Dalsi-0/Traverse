using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject worldUI; 
    private LayerMask playerLayer;
    private float checkInterval = 0.05f;

    private void Start()
    {
        worldUI.SetActive(false);
        playerLayer = GameManager.Instance.GetGameReferences().PlayerLayer;
        StartCoroutine(CheckForPlayer());
    }

    private IEnumerator CheckForPlayer()
    {
        while (true)
        {
            RaycastHit hit;
            Debug.DrawLine(transform.position, transform.position + transform.forward * 100f, Color.red);

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, playerLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    worldUI.SetActive(true);
                    break;
                }
            }

            yield return new WaitForSeconds(checkInterval); 
        }
    }
}
