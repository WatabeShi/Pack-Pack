using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TitleModeChange : MonoBehaviour
{
    private bool isFPS = false;

    [Header("モード切替時に移動させるUI"), SerializeField]
    private RectTransform ui;
    private CanvasGroup uiGroup;

    [Header("切り替えボタンの回転数"), SerializeField]
    private float rotationAmount = 1;

    [Header("モード切替ボタン"), SerializeField]
    private RectTransform changeButton;
    private Text changeButtonText;

    private bool changeTrigger = true;

    [Header("アニメーション時間"), SerializeField]
    private float animTime;

    [Header("ボタン押下時のSE"), SerializeField]
    private AudioClip clickSE;
    [Header("モード切り替え時のSE"), SerializeField]
    private AudioClip modeChangeSE;
    [Header("BGM(Third Person)"), SerializeField]
    private AudioClip bgmTPS;
    [Header("BGM(First Person)"), SerializeField]
    private AudioClip bgmFPS;

    private void Awake()
    {
        changeButtonText = changeButton.transform.GetChild(0).GetComponent<Text>();   // ボタンの子オブジェクトのテキストを取得

        uiGroup = ui.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        BGMSource.instance.PlayBGM(bgmTPS);
    }

    public void ChangeMode()   // モードによってカメラ等を回転させる
    {
        if (changeTrigger == false) return;

        changeTrigger = false;

        SESource.instance.PlaySE(clickSE);   // ボタンクリックSE
        DOVirtual.DelayedCall(animTime / 2, () => SESource.instance.PlaySE(modeChangeSE));   // 切り替えSEの遅延再生

        AnimationUI();   // UIのアニメーション

        isFPS = !isFPS;   // モードの切り替え

        DOVirtual.DelayedCall(animTime, () => ChangeBGM());   // BGM 切り替え

        changeButtonText.text = isFPS ? "First Person" : "Third Person";   // 状態に応じてボタンのテキストを変更
    }

    private void ChangeBGM()
    {
        BGMSource.instance.StopBGM(0.5f);

        AudioClip _bgm = isFPS ? bgmFPS : bgmTPS;
        DOVirtual.DelayedCall(1, () => BGMSource.instance.PlayBGM(_bgm));
    }

    private void AnimationUI()   // アニメーション
    {
        // 自オブジェクトの回転
        var _rotation = isFPS ? new Vector3(0, 0, 0) : new Vector3(0, 0, -180);   // 角度の切り替え
        this.transform.DORotate(_rotation, animTime).SetEase(Ease.InOutQuart);

        // UIの移動
        var _pos = isFPS ? new Vector2(-500, 170) : new Vector2(500, -50);   // 移動位置の切り替え
        DOTween.Sequence()
            .Append(uiGroup.DOFade(0, animTime / 2))
            .Append(ui.DOAnchorPos(_pos, 0))
            .Append(uiGroup.DOFade(1, animTime / 2));

        // ボタンの回転と拡縮
        var _rotationAmount = new Vector3(0, 0, -360 * rotationAmount);   // 回転数
        DOTween.Sequence()
            .Append(changeButton.DOScale(new Vector2(1.25f, 1.25f), animTime / 2))   // 拡大
            .Append(changeButton.DOScale(new Vector2(1, 1), animTime / 2))           // 縮小
            .Insert(0, changeButton.DOLocalRotate(_rotationAmount/* * 3*/, animTime, RotateMode.FastBeyond360))   // 回転
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
