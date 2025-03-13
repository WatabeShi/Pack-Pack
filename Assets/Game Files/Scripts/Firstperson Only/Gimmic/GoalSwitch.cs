using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class GoalSwitch : MonoBehaviour
{
    private bool isOpen = false;

    [Header("�S�[���h�A"), SerializeField]
    private GameObject goalDoor;

    [Header("�X�C�b�`�������ꂽ�Ƃ���SE�i���������Ă��鎞�j"), SerializeField]
    private AudioClip pushedSE;
    [Header("�X�C�b�`�������ꂽ�Ƃ���SE�i���������Ă��Ȃ����j"), SerializeField]
    private AudioClip pushedErrorSE;
    [Header("�h�A���J������SE"), SerializeField]
    private AudioClip openSE;

    public void OpenGoal(bool hasKey)
    {
        if (isOpen) return;

        var _pushedSE = hasKey ? pushedSE : pushedErrorSE;   // ���������Ă��邩�Ŗ炷SE��ς���
        SESource.instance.PlaySE(_pushedSE);   // �X�C�b�`�������ꂽ�Ƃ���SE��炷

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
