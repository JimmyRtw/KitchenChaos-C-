using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartContdownUI : MonoBehaviour
{
    private const string NUMBER_POP_UP = "NumberPopUp";
    [SerializeField] private TextMeshProUGUI countDownText;

    private Animator animator;
    private int previousCountdownNumber;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanaged;

        Hide();
    }

    private void Update()
    {
        int countDownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountDownToStartTimer());
        countDownText.text = countDownNumber.ToString();

        if(previousCountdownNumber!=countDownNumber)
        {
            previousCountdownNumber = countDownNumber;
            animator.SetTrigger(NUMBER_POP_UP);
            SoundManager.Instance.PlayCountDownSound();
        }
    }

    private void KitchenGameManager_OnStateChanaged(object sender, EventArgs e)
    {
        if(KitchenGameManager.Instance.IsCountDownToStartActive())
        {
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
