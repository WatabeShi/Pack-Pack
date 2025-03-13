using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEVolumeSlider : VolumeSlider
{
    private void Start()
    {
        audioMixer.GetFloat("SE", out float seVolume);
        var volume = Mathf.Pow(10f, seVolume / 20f);
        volumeSlider.value = volume;   // �X���C�_�[�̍ŏ��̒l��ݒ�
    }

    public override void SetAudioMixier(float volume)
    {
        audioMixer.SetFloat("SE", ConvertVolumeToDb(volumeSlider.value));
        print("SE : " + volume);
    }
}
