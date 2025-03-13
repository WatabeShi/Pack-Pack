using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [Header("�f�t�H���g����"), Range(0.0f, 1.0f), SerializeField]
    protected float defaultVolume;

    protected Slider volumeSlider;

    [Header("�l��ς�������SE"), SerializeField]
    protected AudioClip changedSE;

    //protected AudioSource audioSource;
    [Header("�g�p����I�[�f�B�I�~�L�T�["), SerializeField]
    protected AudioMixer audioMixer;

    protected void Awake()
    {
        volumeSlider = GetComponent<Slider>();
    }

    public virtual void SetAudioMixier(float volume) { }

    protected float ConvertVolumeToDb(float volume)   // �{�����[�����f�V�x���ɕϊ����ĕԂ�
    {
        return Mathf.Clamp(Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)) * 20f, -80f, 0f);
    }

    public void SeWhenChengedVolume(bool isSESource)
    {
        if (isSESource) SESource.instance.PlaySE(changedSE);
        else BGMSource.instance.PlaySE(changedSE);
    }
}
