using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SESource : MonoBehaviour
{
    public static SESource instance;

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

    public void PlaySE(AudioClip _se)   // SE‚ð–Â‚ç‚·
    {
        if (_se == null) return;

        audioSource.PlayOneShot(_se);
    }
}
