using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int startScoreHigh;   // ゲーム開始時点でのハイスコア
    public static int scoreHigh = 0;    // ハイスコア
    public static int scoreNow = 0;     // 現在のスコア

    [Header("ハイスコアを表示するテキスト"), SerializeField]
    private Text scoreHighText;
    [Header("現在のスコアを表示するテキスト"), SerializeField]
    private Text scoreNowText;

    //TextAsset csvFile;
    //List<string[]> csvDatas = new List<string[]>();

    // Start is called before the first frame update
    protected void Start()
    {
        // 最初のステージの時のみハイスコアを読み込み
        if (GameManagerTPS.stageNum == 1) GetHighScore();
        //scoreHigh = 0;

        UpdateScoreText();   // スコア表示

        if (scoreHighText != null) scoreHighText.text = scoreHigh.ToString("00000");   // ハイスコア表示
    }

    public void AddScore(int _score)   // 点数追加
    {
        scoreNow += _score;

        if (scoreNow > scoreHigh) scoreHigh = scoreNow;

        UpdateScoreText();
        UpdateHighScoreText();
    }

    private void UpdateScoreText()   // 現在のスコアテキストの更新
    {
        if (scoreNowText == null) return;

        // 現在のスコアの表示の更新
        scoreNowText.text = scoreNow.ToString("00000");
    }

    private void UpdateHighScoreText()   // ハイスコアテキストの更新
    {
        if (scoreHighText == null) return;

        // 現在のスコアがハイスコアを超えた場合、ハイスコアの表示を更新
        if (scoreNow < scoreHigh) return;

        scoreHighText.text = scoreNow.ToString("00000");   // ハイスコアテキストの更新

        PlayerPrefs.SetInt("High Score", scoreHigh);   // ハイスコアを保存

        //var sw = new StreamWriter("Assets/Resources/High Score.csv", false/*, Encoding.GetEncoding("Shift_JIS")*/);   // 文字コードがshift_JISの場合ビルドすると正常に動かなくなるかも
        //sw.Write(ScoreManager.scoreHigh);   // ハイスコアを記録する
        //sw.Close();  
    }

    private void GetHighScore()   // ハイスコア取得
    {
        scoreHigh = PlayerPrefs.GetInt("High Score", 0);   // ハイスコアを取り出し
        startScoreHigh = scoreHigh;                        // ゲーム開始時点のハイスコアを保存

        //var txtFile = Resources.Load("High Score") as TextAsset;
        //StringReader reader = new StringReader(txtFile.text);

        //string line = reader.ReadLine();

        //scoreHigh = int.Parse(line);
        //print(line);

        //while (reader.Peek() != -1)
        //{
        //    string line = reader.ReadLine();
        //    csvDatas.Add(line.Split(','));
        //}

        //for (int i = 0; i < csvDatas.Count; i++)
        //{
        //    for (int j = 0; j < csvDatas[i].Length; j++)
        //    {
        //        print(csvDatas[i][j]);

        //        scoreHigh = int.Parse(csvDatas[i][j]);
        //    }
        //}
    }
}
