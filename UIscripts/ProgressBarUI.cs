using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        if(hasProgress==null)
        {
            Debug.LogError("Null reference execption!! on has progress component!!");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0;

        HideObject();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if(e.progressNormalized==0f || e.progressNormalized==1f)
        {
            HideObject();
        }
        else
        {
            ShowObject();
        }
    }

    private void ShowObject()
    {
        gameObject.SetActive(true);
    }
    private void HideObject()
    {
        gameObject.SetActive(false);
    }
}
