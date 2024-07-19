using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance {get;private set;}
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameUnpaused;
    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float countdownToStartTimer = 5f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 120f;
    private bool isGamePause = false;

    private void Start()
    {
        GameInput.Instance.OnPause += GameInput_OnPause;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
     }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            state = State.CountDownToStart;
            OnStateChanged ? .Invoke(this,EventArgs.Empty);
        }
    }

    private void GameInput_OnPause(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Update()
    {
        switch(state)
        {
            case State.WaitingToStart :

            break;

            case State.CountDownToStart :

                countdownToStartTimer -= Time.deltaTime;

                if(countdownToStartTimer<0)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged ? .Invoke(this,EventArgs.Empty);
                }

            break;

            case State.GamePlaying :

                gamePlayingTimer -= Time.deltaTime;

                if(gamePlayingTimer<0)
                {
                    state = State.GameOver;
                    OnStateChanged ? .Invoke(this,EventArgs.Empty);
                }

            break;


            case State.GameOver :

            break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountDownToStartActive()
    {
        return state == State.CountDownToStart;
    }

    public float GetCountDownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimeNormalized()
    {
        return 1 - (gamePlayingTimer/gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isGamePause = !isGamePause;

        if(isGamePause)
        {
            OnGamePause ? .Invoke(this,EventArgs.Empty);
            Time.timeScale = 0f;
        }
        else
        {
            OnGameUnpaused? .Invoke(this,EventArgs.Empty);
            Time.timeScale = 1f;
        }
    }
}
