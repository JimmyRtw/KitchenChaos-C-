using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footStepTimer;
    private float footStepTimerMax = 0.1f;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        footStepTimer -= Time.deltaTime;

        if(footStepTimer<0)
        {
            footStepTimer = footStepTimerMax;
            
            if(player.IsWalking())
            {
                SoundManager.Instance.PlayFootStepSound(player.transform.position);
            }
        }
    }
}
