using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class GoalSwitch : MonoBehaviour
{
    private bool isOpen = false;

    [Header("ゴールドア"), SerializeField]
    private GameObject goalDoor;

    [Header("スイッチが押されたときのSE（鍵を持っている時）"), SerializeField]
    private AudioClip pushedSE;
    [Header("スイッチが押されたときのSE（鍵を持っていない時）"), SerializeField]
    private AudioClip pushedErrorSE;
    [Header("ドアが開く時のSE"), SerializeField]
    private AudioClip openSE;

    public void OpenGoal(bool hasKey)
    {
        if (isOpen) return;

        var _pushedSE = hasKey ? pushedSE : pushedErrorSE;   // 鍵を持っているかで鳴らすSEを変える
        SESource.instance.PlaySE(_pushedSE);   // スイッチが押されたときのSEを鳴らす

        if (!hasKey) return;

        isOpen = true;
        goalDoor.transform.DOMoveY(12.5f, 2f);

        SESource.instance.PlaySE(openSE);
    }

    private void OnDestroy()
    {
        DOTween.Clear(true);
    }
}
