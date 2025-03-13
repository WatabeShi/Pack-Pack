using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMVolumeSlider : VolumeSlider
{
    private void Start()
    {
        audioMixer.GetFloat("BGM", out float bgmVolume);
        var volume = Mathf.Pow(10f, bgmVolume / 20f);
        volumeSlider.value = volume;   // スライダーの最初の値を設定
    }

    public override void SetAudioMixier(float volume)
    {
        audioMixer.SetFloat("BGM", ConvertVolumeToDb(volumeSlider.value));
        print("BGM : " + volume);
    }
}
