using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> KitchenObjectSOGameObjectsList;

    private void Start()
    {
        plateKitchenObject.onIndgredientAdded += PlateKitchenObject_OnIndgredientAdded;

        foreach(KitchenObjectSO_GameObject kitchenObjectSOGameObject in KitchenObjectSOGameObjectsList)
        {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        } 
    }

    private void PlateKitchenObject_OnIndgredientAdded(object sender, PlateKitchenObject.OnIndgredientAddedEventArgs e)
    {
       foreach(KitchenObjectSO_GameObject kitchenObjectSOGameObject in KitchenObjectSOGameObjectsList)
       {
            if(kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
       } 
    }
}
