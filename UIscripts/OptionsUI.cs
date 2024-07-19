using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance {get;private set;}
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI moveUpButtonText;
    [SerializeField] private TextMeshProUGUI moveDownButtonText;
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private TextMeshProUGUI interactAltButtonText;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private Transform pressToRebindKeyTransform;


    private void Start()
    {  
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void Awake()
    {
        Instance = this;

        soundEffectsButton.onClick.AddListener(()=>{
            SoundManager.Instance.ChanageVolume();
            UpdateVisual();
        });
        
        musicButton.onClick.AddListener(()=>{
            MusicManager.Instance.ChanageVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(()=>{
            Hide();
        });

        moveUpButton.onClick.AddListener(()=>{ RebindBinding(GameInput.Bindings.Move_Up);});
        moveDownButton.onClick.AddListener(()=>{ RebindBinding(GameInput.Bindings.Move_Down);});
        moveLeftButton.onClick.AddListener(()=>{ RebindBinding(GameInput.Bindings.Move_Left);});
        moveRightButton.onClick.AddListener(()=>{ RebindBinding(GameInput.Bindings.Move_Right);});
        interactButton.onClick.AddListener(()=>{ RebindBinding(GameInput.Bindings.Interact);});
        interactAltButton.onClick.AddListener(()=>{ RebindBinding(GameInput.Bindings.Interact_Alternate);});
        pauseButton.onClick.AddListener(()=>{ RebindBinding(GameInput.Bindings.Pause);});
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "GAME VOLUME : " + Mathf.Round(SoundManager.Instance.GetVolume()*10f); 
        musicText.text = "MUSIC : " + Mathf.Round(MusicManager.Instance.GetVolume()*10f); 

        moveUpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Up);
        moveDownButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Down);
        moveLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Left);
        moveRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Right);
        interactButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact);
        interactAltButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact_Alternate);
        pauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Bindings bindings)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(bindings,()=>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
