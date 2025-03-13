using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreGetAndDelete : MonoBehaviour
{
    [Header("�n�C�X�R�A�\���e�L�X�g"), SerializeField]
    private Text highScoreText;

    private void Start()
    {
        GetHighScore();
    }

    private void Update()
    {
        highScoreText.text = ScoreManager.scoreHigh.ToString("00000");
    }

    public void DeleteHighScore()   // �n�C�X�R�A�폜���ēǂݍ���
    {
        PlayerPrefs.DeleteKey("High Score");
        GetHighScore();
    }

    private void GetHighScore()   // �n�C�X�R�A�ǂݍ���
    {
        ScoreManager.scoreHigh = PlayerPrefs.GetInt("High Score", 00000);
    }
}
