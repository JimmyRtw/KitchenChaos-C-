using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;
    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        plateKitchenObject.onIndgredientAdded += PlateKitchenObject_OnIndgredientAdded;
    }

    private void PlateKitchenObject_OnIndgredientAdded(object sender, PlateKitchenObject.OnIndgredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach(Transform child in transform)
        {
            if(child==iconTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach(KitchenObjectSO kitchenObjectSo in plateKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate,transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSo(kitchenObjectSo);
        }
    }
}
