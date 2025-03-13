using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSource : MonoBehaviour
{
    public static BGMSource instance;

    private float playTime = 0;   // 再生位置保存用

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// BGM再生（音源変更なし）
    /// </summary>
    public void PlayBGM()
    {
        if (audioSource.clip == null) return;

        audioSource.time = playTime;
        audioSource.Play();
    }

    /// <summary>
    /// BGM再生（音源変更あり）
    /// </summary>
    /// <param name="_bgm">再生したい音源</param>
    public void PlayBGM(AudioClip _bgm)
    {
        if (_bgm == null) return;

        if (audioSource.clip != null) StopBGM();   // 既にクリップがある場合停止

        playTime = 0;   // 再生位置リセット

        audioSource.clip = _bgm;
        audioSource.Play();
    }

    /// <summary>
    /// BGM再生（音源変更あり、再生位置指定あり）
    /// </summary>
    /// <param name="_bgm">再生したい音源</param>
    /// <param name="startTime">再生を始める位置</param>
    public void PlayBGM(AudioClip _bgm, float startTime)
    {
        if (_bgm == null) return;

        if (audioSource.clip != null) StopBGM();   // 既にクリップがある場合停止

        playTime = 0;   // 再生位置リセット

        audioSource.clip = _bgm;
        audioSource.Play();
    }

    /// <summary>
    /// BGM再生（音源変更あり、再生位置指定あり、音量のフェードインあり）
    /// </summary>
    /// <param name="_clip">再生したい音源</param>
    /// <param name="startTime">再生を始める位置</param>
    /// <param name="fadeTime">フェードインにかける時間</param>
    public void PlayBGM(AudioClip _clip, float startTime, float fadeTime)
    {
        StartCoroutine(PlayBGMwithFade(_clip, fadeTime, startTime));
    }

    private IEnumerator PlayBGMwithFade(AudioClip _clip, float _fadeTime, float _startTime)
    {
        audioSource.volume = 0;

        audioSource.clip = _clip;
        audioSource.Play();

        audioSource.time = _startTime;   // 再生位置を設定

        var fadeAmount = 1 / _fadeTime;   // 一秒間で増加する音量の計算

        while (audioSource.volume < 1)
        {
            audioSource.volume += fadeAmount * Time.deltaTime;   // 計算した増加量を掛け合わせる

            yield return null;
        }
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBGM()
    {
        if (audioSource.clip == null) return;

        playTime = audioSource.time;   // 再生時間の保存
        audioSource.Stop();
    }

    /// <summary>
    /// BGM停止（フェードアウトさせる）
    /// </summary>
    /// <param name="fadeTime"></param>
    public void StopBGM(float fadeTime)
    {
        StartCoroutine(StopBGMwithFade(fadeTime));
    }

    private IEnumerator StopBGMwithFade(float _fadeTime)
    {
        var fadeAmount = 1 / _fadeTime;   // 一秒間で減少する音量

        while (audioSource.volume > 0)
        {
            audioSource.volume -= fadeAmount * Time.deltaTime;

            yield return null;
        }

        StopBGM();
        RemoveClip();
        audioSource.volume = 1;
    }

    /// <summary>
    /// 現在セットされている音源の除去
    /// </summary>
    public void RemoveClip()
    {
        audioSource.clip = null;
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="_se">再生したい音源</param>
    public void PlaySE(AudioClip _se)
    {
        if (_se == null) return;

        audioSource.PlayOneShot(_se);
    }
}
