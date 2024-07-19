using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    private void Awake()
    {
        mainMenuButton.onClick.AddListener(()=>{

            Loader.Load(Loader.Scene.MainMenuScene);

            Time.timeScale = 1f;
        });

        resumeButton.onClick.AddListener(()=>{

            KitchenGameManager.Instance.TogglePauseGame();
        });

         optionsButton.onClick.AddListener(()=>{

            OptionsUI.Instance.Show();
        });       
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePause += KitchenGameManager_OnGamePause;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide(); 
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnGamePause(object sender, EventArgs e)
    {
        Show();
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
