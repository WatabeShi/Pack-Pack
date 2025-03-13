using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TitleModeChange : MonoBehaviour
{
    private bool isFPS = false;

    [Header("���[�h�ؑ֎��Ɉړ�������UI"), SerializeField]
    private RectTransform ui;
    private CanvasGroup uiGroup;

    [Header("�؂�ւ��{�^���̉�]��"), SerializeField]
    private float rotationAmount = 1;

    [Header("���[�h�ؑփ{�^��"), SerializeField]
    private RectTransform changeButton;
    private Text changeButtonText;

    private bool changeTrigger = true;

    [Header("�A�j���[�V��������"), SerializeField]
    private float animTime;

    [Header("�{�^����������SE"), SerializeField]
    private AudioClip clickSE;
    [Header("���[�h�؂�ւ�����SE"), SerializeField]
    private AudioClip modeChangeSE;
    [Header("BGM(Third Person)"), SerializeField]
    private AudioClip bgmTPS;
    [Header("BGM(First Person)"), SerializeField]
    private AudioClip bgmFPS;

    private void Awake()
    {
        changeButtonText = changeButton.transform.GetChild(0).GetComponent<Text>();   // �{�^���̎q�I�u�W�F�N�g�̃e�L�X�g���擾

        uiGroup = ui.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        BGMSource.instance.PlayBGM(bgmTPS);
    }

    public void ChangeMode()   // ���[�h�ɂ���ăJ����������]������
    {
        if (changeTrigger == false) return;

        changeTrigger = false;

        SESource.instance.PlaySE(clickSE);   // �{�^���N���b�NSE
        DOVirtual.DelayedCall(animTime / 2, () => SESource.instance.PlaySE(modeChangeSE));   // �؂�ւ�SE�̒x���Đ�

        AnimationUI();   // UI�̃A�j���[�V����

        isFPS = !isFPS;   // ���[�h�̐؂�ւ�

        DOVirtual.DelayedCall(animTime, () => ChangeBGM());   // BGM �؂�ւ�

        changeButtonText.text = isFPS ? "First Person" : "Third Person";   // ��Ԃɉ����ă{�^���̃e�L�X�g��ύX
    }

    private void ChangeBGM()
    {
        BGMSource.instance.StopBGM(0.5f);

        AudioClip _bgm = isFPS ? bgmFPS : bgmTPS;
        DOVirtual.DelayedCall(1, () => BGMSource.instance.PlayBGM(_bgm));
    }

    private void AnimationUI()   // �A�j���[�V����
    {
        // ���I�u�W�F�N�g�̉�]
        var _rotation = isFPS ? new Vector3(0, 0, 0) : new Vector3(0, 0, -180);   // �p�x�̐؂�ւ�
        this.transform.DORotate(_rotation, animTime).SetEase(Ease.InOutQuart);

        // UI�̈ړ�
        var _pos = isFPS ? new Vector2(-500, 170) : new Vector2(500, -50);   // �ړ��ʒu�̐؂�ւ�
        DOTween.Sequence()
            .Append(uiGroup.DOFade(0, animTime / 2))
            .Append(ui.DOAnchorPos(_pos, 0))
            .Append(uiGroup.DOFade(1, animTime / 2));

        // �{�^���̉�]�Ɗg�k
        var _rotationAmount = new Vector3(0, 0, -360 * rotationAmount);   // ��]��
        DOTween.Sequence()
            .Append(changeButton.DOScale(new Vector2(1.25f, 1.25f), animTime / 2))   // �g��
            .Append(changeButton.DOScale(new Vector2(1, 1), animTime / 2))           // �k��
            .Insert(0, changeButton.DOLocalRotate(_rotationAmount/* * 3*/, animTime, RotateMode.FastBeyond360))   // ��]
            .OnComplete(() => changeTrigger = true);
    }

    public bool IsFPS
    {
        get { return isFPS; }
    }

    private void OnDestroy()
    {
        DOTween.Clear(true);
    }
}
