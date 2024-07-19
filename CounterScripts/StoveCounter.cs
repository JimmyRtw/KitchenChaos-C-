using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoveCounter : BaseCounter,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;   
    public event EventHandler<onStateChangedEventArgs> onStateChanged;

    public class onStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float buriningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if(HasKitchenObject())
        {
            switch(state)
            {
                case State.Idle :
                    break;

                case State.Frying : 

                    fryingTimer += Time.deltaTime;

                    OnProgressChanged ? .Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)fryingTimer/fryingRecipeSO.fryingTimerMax
                    });

                    if(fryingRecipeSO.fryingTimerMax<fryingTimer)
                    {
                        GetKitchenObject().DestroySelft();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output,this);

                        state = State.Fried;

                        buriningTimer = 0f;

                        burningRecipeSO = GetBurningOuputForInputSo(GetKitchenObject().GetKitchenObjectSO());

                        onStateChanged ? .Invoke(this,new onStateChangedEventArgs
                        {
                            state = state
                        });

                    }

                    break;

                case State.Fried : 

                   buriningTimer += Time.deltaTime; 

                    OnProgressChanged ? .Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)buriningTimer/burningRecipeSO.buriningTimerMax
                    });

                   if(burningRecipeSO.buriningTimerMax<buriningTimer)
                   {
                        GetKitchenObject().DestroySelft();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output,this);

                        state = State.Burned;

                        onStateChanged ? .Invoke(this,new onStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged ? .Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }

                    break;

                case State.Burned : 

                    break;
            }
        }

    }
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())  // table doesn't contain any object
        {
            // player has something in his/her hand and that can be Fried than we drop it on stove counter
            if(player.HasKitchenObject() && HasRecipeFromInput(player.GetKitchenObject().GetKitchenObjectSO())) 
            {
                player.GetKitchenObject().SetkitchenObjectParent(this);

                fryingRecipeSO = GetFryingOuputForInputSo(GetKitchenObject().GetKitchenObjectSO());

                state = State.Frying;
                fryingTimer = 0f;

                onStateChanged ? .Invoke(this,new onStateChangedEventArgs
                {
                    state = state
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

                state = State.Idle;

                onStateChanged ? .Invoke(this,new onStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged ? .Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
            else // player have something 
            {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelft();

                         state = State.Idle;

                        onStateChanged ? .Invoke(this,new onStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged ? .Invoke(this,new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }

                }
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO inputFryingRecipeSO = GetFryingOuputForInputSo(inputKitchenObjectSO);

        if(inputFryingRecipeSO!=null) return (KitchenObjectSO) inputFryingRecipeSO.output;

        return null;
    }

    private bool HasRecipeFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingOuputForInputSo(inputKitchenObjectSO) != null;
    }   

    private FryingRecipeSO GetFryingOuputForInputSo(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if(fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }

         return null;        
    }

    private BurningRecipeSO GetBurningOuputForInputSo(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if(burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }

         return null;        
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
