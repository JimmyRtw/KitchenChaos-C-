using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveGameObject;
    [SerializeField] private GameObject particalsGameObject;

    private void Start()
    {
        stoveCounter.onStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.onStateChangedEventArgs e)
    {
        bool visualFlag = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying;

        stoveGameObject.SetActive(visualFlag);
        particalsGameObject.SetActive(visualFlag);
    }
}
