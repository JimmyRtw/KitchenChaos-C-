using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using TMPro;
using UnityEngine;

public class DeliveryManangerSingleUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeName.text = recipeSO.name;

        foreach(Transform child in iconContainer)
        {
            if(child==iconTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach(KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform iconTemplateTransform =  Instantiate(iconTemplate,iconContainer);
            iconTemplateTransform.gameObject.SetActive(true);
            iconTemplateTransform.GetComponent<UnityEngine.UI.Image>().sprite = kitchenObjectSO.sprite;
        }
    }

}

