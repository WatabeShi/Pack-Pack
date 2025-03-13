using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int startScoreHigh;   // �Q�[���J�n���_�ł̃n�C�X�R�A
    public static int scoreHigh = 0;    // �n�C�X�R�A
    public static int scoreNow = 0;     // ���݂̃X�R�A

    [Header("�n�C�X�R�A��\������e�L�X�g"), SerializeField]
    private Text scoreHighText;
    [Header("���݂̃X�R�A��\������e�L�X�g"), SerializeField]
    private Text scoreNowText;

    //TextAsset csvFile;
    //List<string[]> csvDatas = new List<string[]>();

    // Start is called before the first frame update
    protected void Start()
    {
        // �ŏ��̃X�e�[�W�̎��̂݃n�C�X�R�A��ǂݍ���
        if (GameManagerTPS.stageNum == 1) GetHighScore();
        //scoreHigh = 0;

        UpdateScoreText();   // �X�R�A�\��

        if (scoreHighText != null) scoreHighText.text = scoreHigh.ToString("00000");   // �n�C�X�R�A�\��
    }

    public void AddScore(int _score)   // �_���ǉ�
    {
        scoreNow += _score;

        if (scoreNow > scoreHigh) scoreHigh = scoreNow;

        UpdateScoreText();
        UpdateHighScoreText();
    }

    private void UpdateScoreText()   // ���݂̃X�R�A�e�L�X�g�̍X�V
    {
        if (scoreNowText == null) return;

        // ���݂̃X�R�A�̕\���̍X�V
        scoreNowText.text = scoreNow.ToString("00000");
    }

    private void UpdateHighScoreText()   // �n�C�X�R�A�e�L�X�g�̍X�V
    {
        if (scoreHighText == null) return;

        // ���݂̃X�R�A���n�C�X�R�A�𒴂����ꍇ�A�n�C�X�R�A�̕\�����X�V
        if (scoreNow < scoreHigh) return;

        scoreHighText.text = scoreNow.ToString("00000");   // �n�C�X�R�A�e�L�X�g�̍X�V

        PlayerPrefs.SetInt("High Score", scoreHigh);   // �n�C�X�R�A��ۑ�

        //var sw = new StreamWriter("Assets/Resources/High Score.csv", false/*, Encoding.GetEncoding("Shift_JIS")*/);   // �����R�[�h��shift_JIS�̏ꍇ�r���h����Ɛ���ɓ����Ȃ��Ȃ邩��
        //sw.Write(ScoreManager.scoreHigh);   // �n�C�X�R�A���L�^����
        //sw.Close();  
    }

    private void GetHighScore()   // �n�C�X�R�A�擾
    {
        scoreHigh = PlayerPrefs.GetInt("High Score", 0);   // �n�C�X�R�A�����o��
        startScoreHigh = scoreHigh;                        // �Q�[���J�n���_�̃n�C�X�R�A��ۑ�

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
