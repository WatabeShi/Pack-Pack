using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreGetAndDelete : MonoBehaviour
{
    [Header("ハイスコア表示テキスト"), SerializeField]
    private Text highScoreText;

    private void Start()
    {
        GetHighScore();
    }

    private void Update()
    {
        highScoreText.text = ScoreManager.scoreHigh.ToString("00000");
    }

    public void DeleteHighScore()   // ハイスコア削除＆再読み込み
    {
        PlayerPrefs.DeleteKey("High Score");
        GetHighScore();
    }

    private void GetHighScore()   // ハイスコア読み込み
    {
        ScoreManager.scoreHigh = PlayerPrefs.GetInt("High Score", 00000);
    }
}
