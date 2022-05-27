using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //objects for holding objects
    [SerializeField] Transform belt;
    [SerializeField] Transform backpack;

    private CollectibleItem[] itemsOnBelt = new CollectibleItem[9];
    private int beltSize = 9;
    private int currentEquipedItemSlotNumber = -1;

    public CollectibleItem EquipItemFromBelt(int slotNumber, Transform parent)
    {
        if (slotNumber < beltSize && slotNumber != currentEquipedItemSlotNumber&& itemsOnBelt[slotNumber] != null)
        {
            if (currentEquipedItemSlotNumber >= 0)   //there is equipped item
            {
                UnequipItemFormHand(currentEquipedItemSlotNumber);
            }

            itemsOnBelt[slotNumber].gameObject.SetActive(true);
            itemsOnBelt[slotNumber].transform.parent = parent;
            itemsOnBelt[slotNumber].transform.SetPositionAndRotation(parent.position, parent.rotation);
            currentEquipedItemSlotNumber = slotNumber;
            return itemsOnBelt[slotNumber];
        }
        else
        {
            return null;
        }
    }

    public void UnequipItemFormHand(int slotNumber)
    {
        if (slotNumber == currentEquipedItemSlotNumber)
        {
            if (itemsOnBelt[slotNumber] != null)
            {
                itemsOnBelt[slotNumber].gameObject.SetActive(false);
                itemsOnBelt[slotNumber].transform.parent = belt;
                currentEquipedItemSlotNumber = -1;
            }
        }
    }

    public void PickUpItem(CollectibleItem item)
    {
        //check for empty slots on belt
        for (int i = 1; i <= beltSize; i ++)
        {
            if (itemsOnBelt[i] == null)
            {
                itemsOnBelt[i] = item;
                Rigidbody _RB = itemsOnBelt[i].GetComponent<Rigidbody>();
                if (_RB != null)
                {
                    _RB.isKinematic = true;
                }
                Collider _col = itemsOnBelt[i].GetComponent<Collider>();
                if (_col != null)
                {
                    _col.enabled = false;
                }
                itemsOnBelt[i].gameObject.SetActive(false);
                itemsOnBelt[i].transform.parent = belt;
                return;
            }
        }
    }

    public CollectibleItem DropItemInHand()
    {
        CollectibleItem item = itemsOnBelt[currentEquipedItemSlotNumber];
        if (currentEquipedItemSlotNumber >= 0 && item != null)
        {
            item.transform.parent = null;
            Rigidbody _RB = item.GetComponent<Rigidbody>();
            if (_RB != null)
            {
                _RB.isKinematic = false;
            }
            Collider _col = item.GetComponent<Collider>();
            if (_col != null)
            {
                _col.enabled = true;
            }
            item.gameObject.SetActive(true);
            itemsOnBelt[currentEquipedItemSlotNumber] = null;
            currentEquipedItemSlotNumber = -1;
            return null;
        }
        return null;
    }
}
