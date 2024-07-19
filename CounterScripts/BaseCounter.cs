using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{

    public static event EventHandler onAnyObjectDrop;
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject = null;
    public static void ResetStaticData()
    {
        onAnyObjectDrop = null;
    }
    public virtual void Interact(Player player)
    {
        // Debug.LogError("Interact!!");
    }

    public virtual void InteractAlternate(Player player)
    {
        // Debug.LogError("Interact!!");
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject)
        {
            onAnyObjectDrop ? .Invoke(this,EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }
}
