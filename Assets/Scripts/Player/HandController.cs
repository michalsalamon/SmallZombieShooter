using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(InventoryController))]

public class HandController : MonoBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    private CollectibleItem rightHandEquipedItem;
    //private CollectibleItem leftHandEquipedItem;


    private InventoryController inventory;
    private Camera playerCamera;
    [SerializeField] private LayerMask playerExclusion;

    private void Awake()
    {
        inventory = GetComponent<InventoryController>();
        playerCamera = FindObjectOfType<Camera>();
    }

    public void UseItemPrimary()
    {
        if (rightHandEquipedItem != null)
        {
            rightHandEquipedItem.UseItemPrimary();
        }
    }

    public void StopUsingItemPrimary()
    {
        if (rightHandEquipedItem != null)
        {
            rightHandEquipedItem.StopUsingItemPrimary();
        }
    }

    public void UseItemSecondary()
    {
        if (rightHandEquipedItem != null)
        {
            rightHandEquipedItem.UseItemSecondary();
        }
    }

    public void StopUsingItemSecondary()
    {
        if (rightHandEquipedItem != null)
        {
            rightHandEquipedItem.StopUsingItemSecondary();
        }
    }

    public void EquipItem(int slotNumber)
    {
        CollectibleItem item = inventory.EquipItemFromBelt(slotNumber, rightHand);
        if (item != null)
        {
            rightHandEquipedItem = item;
        }
    }

    public void DropItemInHand()
    {
        if (rightHandEquipedItem != null)
        {
            rightHandEquipedItem = inventory.DropItemInHand();
        }
    }

    public void AimWithItems()
    {
        Vector3 aimPoint = Vector3.zero;
        if (playerCamera != null)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, playerExclusion))
            {
                aimPoint = hit.point;
            }
            else
            {
                aimPoint = playerCamera.transform.position + playerCamera.transform.rotation * Vector3.forward * 15;
            }
        }

        if (rightHandEquipedItem != null)
        {
            rightHandEquipedItem.transform.LookAt(aimPoint);
        }
    }
}
