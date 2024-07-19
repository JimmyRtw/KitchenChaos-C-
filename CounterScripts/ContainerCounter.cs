using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject()) //player doesn't contain anything in his/her hand
        {
            // instantiating the prefab of current kitchen object
            KitchenObject.SpawnKitchenObject(kitchenObjectSO,player);

            OnPlayerGrabbedObject?.Invoke(this,EventArgs.Empty);
        }
    }
}
