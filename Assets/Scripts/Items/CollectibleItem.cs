using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public virtual void UseItemPrimary() { }
    public virtual void StopUsingItemPrimary() { }

    public virtual void UseItemSecondary() { }
    public virtual void StopUsingItemSecondary() { }

    public virtual void IsInteractible()
    {
        Debug.Log("Interactable");
        //todo highlighrting interactable object
    }
}
