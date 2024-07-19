using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverButtonsUI : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(() => {
             Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() => {
             Application.Quit();
        });
    }
}