using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(InventoryController))]

public class InteractionsController : MonoBehaviour
{
    [SerializeField] private LayerMask itemMask;
    [SerializeField] private float interactionRange = 1.5f;

    private Camera playerCamera;
    private InventoryController inventory;

    private void Awake()
    {
        playerCamera = FindObjectOfType<Camera>();
        inventory = GetComponent<InventoryController>();
    }

    public void ActorInteracting()
    {
        Interaction(true);
    }

    public void IsInteractable()
    {
        Interaction(false);
    }

    private void Interaction(bool isInteracting)
    {
        if (playerCamera != null)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionRange, itemMask))
            {
                CollectibleItem item = hit.transform.GetComponent<CollectibleItem>();
                if (item != null)
                {
                    item.IsInteractible();
                    if (isInteracting)
                    {
                        inventory.PickUpItem(item);
                    }
                }
            }
        }
    }
}
