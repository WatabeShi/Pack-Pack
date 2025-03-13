using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class RankingWindow : MonoBehaviour
{
    [SerializeField]
    private bool isOpen = false;   // �\���؂�ւ��t���O

    [Header("���C�����"), SerializeField]
    private GameObject mainScreen;
    [Header("�����L���O�E�B���h�E"), SerializeField]
    private GameObject rankingWindow;   // �I�v�V�������

    [Header("TPS�����L���O�\���e�L�X�g"), SerializeField]
    private Text[] rankTextTPS;
    [Header("FPS�����L���O�\���e�L�X�g"), SerializeField]
    private Text[] rankTextFPS;

    [Header("�{�^���N���b�N����SE"), SerializeField]
    private AudioClip clickSE;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        mainScreen.SetActive(!isOpen);
        rankingWindow.SetActive(isOpen);

        //contents.position = new Vector2(contents.position.x, 0);
    }

    private void Update()
    {
        DispRanking();
    }

    public void ControlWindow()
    {
        isOpen = !isOpen;

        SESource.instance.PlaySE(clickSE);

        mainScreen.SetActive(!isOpen);
        rankingWindow.SetActive(isOpen);
    }

    private void DispRanking()
    {
        ScoreRanking.instance.DispRanking(rankTextTPS, rankTextFPS);
    }

    public void DeleteRanking()
    {
        SESource.instance.PlaySE(clickSE);
        ScoreRanking.instance.DeleteRanking();
    }

    //public bool IsOpen
    //{
    //    get { return isOpen; }
    //}
}
