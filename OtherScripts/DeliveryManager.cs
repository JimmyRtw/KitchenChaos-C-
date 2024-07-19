using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler onRecipeSpawned;
    public event EventHandler onRecipeComplete;
    public event EventHandler onRecipeSuccess;
    public event EventHandler onRecipeFail;

    public static DeliveryManager Instance {get;private set;}
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfullRecipesAmount = 0; 
    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if(spawnRecipeTimer<=0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if(KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count<waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOlist[UnityEngine.Random.Range(0,recipeListSO.recipeSOlist.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                onRecipeSpawned ? .Invoke(this,EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0 ; i < waitingRecipeSOList.Count ; i++) // iterating over all the waiting recipes
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            
            // count of ingredients in the plates doesn't matches with waiting recipes ingredients, we can skip this recipe
            if(waitingRecipeSO.kitchenObjectSOList.Count != plateKitchenObject.GetKitchenObjectSOList().Count) continue;

            bool recipeFound = true;

            foreach(KitchenObjectSO kitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) // iterating over all objects in waiting recipe
            {
                bool ingredientFound = false;

                foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) // iterating over all objects in plate
                {
                    if(plateKitchenObjectSO == kitchenObjectSO)
                    {
                        ingredientFound = true;
                        break;
                    }
                }

                if(!ingredientFound)
                {
                    recipeFound = false;
                }
            }

            if(recipeFound)
            {
                waitingRecipeSOList.RemoveAt(i);
                onRecipeComplete ? .Invoke(this,EventArgs.Empty);
                onRecipeSuccess ? .Invoke(this,EventArgs.Empty);
                successfullRecipesAmount++;
                return;
            }
        }

        onRecipeFail ? .Invoke(this,EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeListSO()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfullRecipesAmount;
    }
}
