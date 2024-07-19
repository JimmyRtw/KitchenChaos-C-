using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpwaned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private float plateSpawnAmount = 0;
    private float plateSpawnAmountMax = 4f;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;

            if(KitchenGameManager.Instance.IsGamePlaying() && plateSpawnAmount<plateSpawnAmountMax)
            {
                plateSpawnAmount++;

                OnPlateSpwaned ? .Invoke(this,EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject()) //player doesn't have anything his/her hand
        {
            if(plateSpawnAmount>0) // counter has atleast one plate
            {
                plateSpawnAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO,player);

                OnPlateRemoved ? .Invoke(this,EventArgs.Empty);
            }
        }
    }
}
