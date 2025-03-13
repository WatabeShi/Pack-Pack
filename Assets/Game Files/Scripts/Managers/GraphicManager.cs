using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicManager : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropDown;

    [Header("ボタンクリック時のSE"), SerializeField]
    private AudioClip clickSE;

    private void Start()
    {
        dropDown.value = 5 - QualitySettings.GetQualityLevel();   // ドロップダウンの値を現在の設定に合わせる
    }

    public void ChangeQuality()   // 画質設定の変更
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
