using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectCount : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] VisualgameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == baseCounter)
        {
            ShowObject();
        }
        else
        {
            HideObject();
        }
    }

    void ShowObject()
    {
        foreach(GameObject visualGameObjectArray in VisualgameObjectArray)
        {
            visualGameObjectArray.SetActive(true);
        }
    }

    void HideObject()
    {
        foreach(GameObject visualGameObjectArray in VisualgameObjectArray)
        {
            visualGameObjectArray.SetActive(false);
        }
    }
}
