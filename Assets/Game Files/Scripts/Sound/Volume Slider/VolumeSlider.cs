using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [Header("デフォルト音量"), Range(0.0f, 1.0f), SerializeField]
    protected float defaultVolume;

    protected Slider volumeSlider;

    [Header("値を変えた時のSE"), SerializeField]
    protected AudioClip changedSE;

    //protected AudioSource audioSource;
    [Header("使用するオーディオミキサー"), SerializeField]
    protected AudioMixer audioMixer;

    protected void Awake()
    {
        volumeSlider = GetComponent<Slider>();
    }

    public virtual void SetAudioMixier(float volume) { }

    protected float ConvertVolumeToDb(float volume)   // ボリュームをデシベルに変換して返す
    {
        return Mathf.Clamp(Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)) * 20f, -80f, 0f);
    }

    public void SeWhenChengedVolume(bool isSESource)
    {
        if (isSESource) SESource.instance.PlaySE(changedSE);
        else BGMSource.instance.PlaySE(changedSE);
    }
}
