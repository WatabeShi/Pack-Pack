using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicManager : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropDown;

    [Header("�{�^���N���b�N����SE"), SerializeField]
    private AudioClip clickSE;

    private void Start()
    {
        dropDown.value = 5 - QualitySettings.GetQualityLevel();   // �h���b�v�_�E���̒l�����݂̐ݒ�ɍ��킹��
    }

    public void ChangeQuality()   // �掿�ݒ�̕ύX
    {
        SESource.instance.PlaySE(clickSE);

        string qualityName = dropDown.options[dropDown.value].text;

        int idx = Array.IndexOf(QualitySettings.names, qualityName);

        if (idx >= 0)
        {
            QualitySettings.SetQualityLevel(idx, true);
            //Debug.Log($"Changed Quality : {quality}");
        }
    }
}
