using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDeliveredText;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanaged;

        Hide();
    }

    private void KitchenGameManager_OnStateChanaged(object sender, EventArgs e)
    {
        if(KitchenGameManager.Instance.IsGameOver())
        {
            recipeDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
