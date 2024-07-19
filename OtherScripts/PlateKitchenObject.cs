using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIndgredientAddedEventArgs> onIndgredientAdded;
    public class OnIndgredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSO;
    private List<KitchenObjectSO> kitchenObjectSOList;

    public void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSO.Contains(kitchenObjectSO)) return false;
        if(kitchenObjectSOList.Contains(kitchenObjectSO)) return false;

        kitchenObjectSOList.Add(kitchenObjectSO);

        onIndgredientAdded ? .Invoke(this,new OnIndgredientAddedEventArgs{
            kitchenObjectSO = kitchenObjectSO
        });

        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
