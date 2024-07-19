using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if(!HasKitchenObject())  // table doesn't contain any object
        {
            if(player.HasKitchenObject()) // player has something in his/her hand
            {
                player.GetKitchenObject().SetkitchenObjectParent(this);
            }
            else // player doesn't have anything so he can't drop anything
            {

            }
        }
        else // table contain any object
        {
            if(!player.HasKitchenObject()) //player doesn't have anything
            {
                // item on the table, we give it to the player
                GetKitchenObject().SetkitchenObjectParent(player);
            }
            else // player have something 
            {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) //player have a plate
                {

                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) //add kitchen object to the plate that player have
                    {
                        GetKitchenObject().DestroySelft(); // destroy the object from table
                    }
                }
                else
                {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) // player doesn't have plate but have something else
                    {
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) // add kitchen object to the plate that table have
                        {
                            player.GetKitchenObject().DestroySelft(); // desstroy the object from player
                        }                        
                    }
                }
            }
        }
    }

}