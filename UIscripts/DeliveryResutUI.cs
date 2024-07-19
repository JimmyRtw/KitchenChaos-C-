#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResutUI : MonoBehaviour
{
    private const string POP_UP = "PopUp";
    [SerializeField] private UnityEngine.UI.Image backgroundImage;
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failSprite;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        DeliveryManager.Instance.onRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.onRecipeFail += DeliveryManager_OnRecipeFail;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFail(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POP_UP);
        backgroundImage.color = failColor;
        iconImage.sprite = failSprite;
        messageText.text = "Delivery\nFail";
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POP_UP);
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.text = "Delivery\nSuccess";       
    }
}

#endif