using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance{get;private set;}
    [SerializeField] private AudioClipRefSO audioClipRefSO;
    private float volume = 1f;
    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME,1f);
    }
    private void Start()
    {
        DeliveryManager.Instance.onRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.onRecipeFail += DeliveryManager_OnRecipeFail;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickUp += Player_OnPickUp;
        BaseCounter.onAnyObjectDrop += BaseCounter_OnAnyObjectDrop;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefSO.objectDrop,trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectDrop(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefSO.objectDrop,baseCounter.transform.position);
    }

    private void Player_OnPickUp(object sender, EventArgs e)
    {
        PlaySound(audioClipRefSO.objectPick,Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefSO.chop,cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFail(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefSO.deliveryFail,deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefSO.deliverySuccess,deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray,Vector3 position,float volume = 1f)
    {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0,audioClipArray.Length)],position,volume);
    }

    private void PlaySound(AudioClip audioClip,Vector3 position,float volumeMultiPlier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip,position,this.volume*volumeMultiPlier);
    }

    public void PlayFootStepSound(Vector3 position,float volume = 1f)
    {
        PlaySound(audioClipRefSO.footStep,position,volume);
    }
    public void PlayCountDownSound()
    {
        float playCountDownVolume = 5f;
        PlaySound(audioClipRefSO.warning,Vector3.zero,playCountDownVolume*volume);
    }

    public void PlayWarningSound(Vector3 position)
    {
        float playCountDownVolume = 5f;
        PlaySound(audioClipRefSO.warning,position,playCountDownVolume*volume);
    }
    public void ChanageVolume()
    {
        this.volume += .1f;

        if(volume > 1f) volume = 0f;

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME,volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
