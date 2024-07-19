using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if(KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact_Alternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Pause);
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
