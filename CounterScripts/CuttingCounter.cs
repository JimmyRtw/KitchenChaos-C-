
using System;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
    public static event EventHandler OnAnyCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;   
    public event EventHandler OnCut;

    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress = 0;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())  // table doesn't contain any object
        {
            // player has something in his/her hand and that can be cut than we drop it on cutting counter
            if(player.HasKitchenObject() && HasRecipeFromInput(player.GetKitchenObject().GetKitchenObjectSO())) 
            {
                cuttingProgress = 0;
                player.GetKitchenObject().SetkitchenObjectParent(this);
                CuttingRecipeSO inputCuttingRecipeSO = GetOuputForInputSo(GetKitchenObject().GetKitchenObjectSO());

                OnProgressChanged ? .Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {    
                    progressNormalized = (float) cuttingProgress / inputCuttingRecipeSO.CuttingProgressMax 
                });
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
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {

                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelft();
                    }
                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        // cutting counter has some kitchen object on it and it can be cut
        if(HasKitchenObject() && HasRecipeFromInput(GetKitchenObject().GetKitchenObjectSO())) 
        {
            cuttingProgress++;
            OnCut ? .Invoke(this,EventArgs.Empty);
            OnAnyCut ? .Invoke(this,EventArgs.Empty);

            CuttingRecipeSO inputCuttingRecipeSO = GetOuputForInputSo(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged ? .Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {    
                progressNormalized = (float) cuttingProgress / inputCuttingRecipeSO.CuttingProgressMax 
            });

            if(cuttingProgress>=inputCuttingRecipeSO.CuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelft(); // destroying kitchen object

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo,this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO inputCuttingRecipeSo = GetOuputForInputSo(inputKitchenObjectSO);

        if(inputCuttingRecipeSo!=null) return (KitchenObjectSO) inputCuttingRecipeSo.output;

        return null;
    }

    private bool HasRecipeFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetOuputForInputSo(inputKitchenObjectSO) != null;
    }   

    private CuttingRecipeSO GetOuputForInputSo(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if(cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }

        return null;        
    }
}
