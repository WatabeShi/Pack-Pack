using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    [Header("最終スコアを表示するテキスト"), SerializeField]
    private Text resultText;
    [Header("ハイスコアの際に表示するテキスト"), SerializeField]
    private GameObject highText;

    [Header("タイトルに戻るまでの時間"), SerializeField]
    private float delayTime = 0;

    [SerializeField]
    private bool isFPS = false;

    // Start is called before the first frame update
    void Start()
    {
        DispResult();   // 最終スコアを表示

        Invoke("BackTitle", delayTime);   // 指定した時間経過後にタイトルへ戻る
    }

    private void DispResult()
    {
        //if (isFPS) ScoreRanking.instance.RankInFPS(ScoreManager.scoreNow);
        //ScoreRanking.instance.RankInTPS(ScoreManager.scoreNow);

        highText.SetActive(false);   // スコア更新テキストを非表示 
        resultText.text = ScoreManager.scoreNow.ToString("00000");   // 最終スコアを表示

        if (ScoreManager.scoreNow <= ScoreManager.startScoreHigh) return;  // 最終スコアがハイスコアを越えた場合先の処理へ

        if(!isFPS)highText.SetActive(true);   // スコア更新テキストを表示
    }

    private void BackTitle()
    {
        BGMSource.instance.StopBGM();
        SceneManager.LoadScene("Title");
    }
}
