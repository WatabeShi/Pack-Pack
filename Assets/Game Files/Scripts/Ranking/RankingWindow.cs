using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class RankingWindow : MonoBehaviour
{
    [SerializeField]
    private bool isOpen = false;   // 表示切り替えフラグ

    [Header("メイン画面"), SerializeField]
    private GameObject mainScreen;
    [Header("ランキングウィンドウ"), SerializeField]
    private GameObject rankingWindow;   // オプション画面

    [Header("TPSランキング表示テキスト"), SerializeField]
    private Text[] rankTextTPS;
    [Header("FPSランキング表示テキスト"), SerializeField]
    private Text[] rankTextFPS;

    [Header("ボタンクリック時のSE"), SerializeField]
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
