using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSource : MonoBehaviour
{
    public static BGMSource instance;

    private float playTime = 0;   // �Đ��ʒu�ۑ��p

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
    /// BGM�Đ��i�����ύX�Ȃ��j
    /// </summary>
    public void PlayBGM()
    {
        if (audioSource.clip == null) return;

        audioSource.time = playTime;
        audioSource.Play();
    }

    /// <summary>
    /// BGM�Đ��i�����ύX����j
    /// </summary>
    /// <param name="_bgm">�Đ�����������</param>
    public void PlayBGM(AudioClip _bgm)
    {
        if (_bgm == null) return;

        if (audioSource.clip != null) StopBGM();   // ���ɃN���b�v������ꍇ��~

        playTime = 0;   // �Đ��ʒu���Z�b�g

        audioSource.clip = _bgm;
        audioSource.Play();
    }

    /// <summary>
    /// BGM�Đ��i�����ύX����A�Đ��ʒu�w�肠��j
    /// </summary>
    /// <param name="_bgm">�Đ�����������</param>
    /// <param name="startTime">�Đ����n�߂�ʒu</param>
    public void PlayBGM(AudioClip _bgm, float startTime)
    {
        if (_bgm == null) return;

        if (audioSource.clip != null) StopBGM();   // ���ɃN���b�v������ꍇ��~

        playTime = 0;   // �Đ��ʒu���Z�b�g

        audioSource.clip = _bgm;
        audioSource.Play();
    }

    /// <summary>
    /// BGM�Đ��i�����ύX����A�Đ��ʒu�w�肠��A���ʂ̃t�F�[�h�C������j
    /// </summary>
    /// <param name="_clip">�Đ�����������</param>
    /// <param name="startTime">�Đ����n�߂�ʒu</param>
    /// <param name="fadeTime">�t�F�[�h�C���ɂ����鎞��</param>
    public void PlayBGM(AudioClip _clip, float startTime, float fadeTime)
    {
        StartCoroutine(PlayBGMwithFade(_clip, fadeTime, startTime));
    }

    private IEnumerator PlayBGMwithFade(AudioClip _clip, float _fadeTime, float _startTime)
    {
        audioSource.volume = 0;

        audioSource.clip = _clip;
        audioSource.Play();

        audioSource.time = _startTime;   // �Đ��ʒu��ݒ�

        var fadeAmount = 1 / _fadeTime;   // ��b�Ԃő������鉹�ʂ̌v�Z

        while (audioSource.volume < 1)
        {
            audioSource.volume += fadeAmount * Time.deltaTime;   // �v�Z���������ʂ��|�����킹��

            yield return null;
        }
    }

    /// <summary>
    /// BGM��~
    /// </summary>
    public void StopBGM()
    {
        if (audioSource.clip == null) return;

        playTime = audioSource.time;   // �Đ����Ԃ̕ۑ�
        audioSource.Stop();
    }

    /// <summary>
    /// BGM��~�i�t�F�[�h�A�E�g������j
    /// </summary>
    /// <param name="fadeTime"></param>
    public void StopBGM(float fadeTime)
    {
        StartCoroutine(StopBGMwithFade(fadeTime));
    }

    private IEnumerator StopBGMwithFade(float _fadeTime)
    {
        var fadeAmount = 1 / _fadeTime;   // ��b�ԂŌ������鉹��

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
    /// ���݃Z�b�g����Ă��鉹���̏���
    /// </summary>
    public void RemoveClip()
    {
        audioSource.clip = null;
    }

    /// <summary>
    /// SE��炷
    /// </summary>
    /// <param name="_se">�Đ�����������</param>
    public void PlaySE(AudioClip _se)
    {
        if (_se == null) return;

        audioSource.PlayOneShot(_se);
    }
}
