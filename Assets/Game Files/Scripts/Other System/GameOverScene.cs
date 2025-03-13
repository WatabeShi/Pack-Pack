using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    [Header("�ŏI�X�R�A��\������e�L�X�g"), SerializeField]
    private Text resultText;
    [Header("�n�C�X�R�A�̍ۂɕ\������e�L�X�g"), SerializeField]
    private GameObject highText;

    [Header("�^�C�g���ɖ߂�܂ł̎���"), SerializeField]
    private float delayTime = 0;

    [SerializeField]
    private bool isFPS = false;

    // Start is called before the first frame update
    void Start()
    {
        DispResult();   // �ŏI�X�R�A��\��

        Invoke("BackTitle", delayTime);   // �w�肵�����Ԍo�ߌ�Ƀ^�C�g���֖߂�
    }

    private void DispResult()
    {
        //if (isFPS) ScoreRanking.instance.RankInFPS(ScoreManager.scoreNow);
        //ScoreRanking.instance.RankInTPS(ScoreManager.scoreNow);

        highText.SetActive(false);   // �X�R�A�X�V�e�L�X�g���\�� 
        resultText.text = ScoreManager.scoreNow.ToString("00000");   // �ŏI�X�R�A��\��

        if (ScoreManager.scoreNow <= ScoreManager.startScoreHigh) return;  // �ŏI�X�R�A���n�C�X�R�A���z�����ꍇ��̏�����

        if(!isFPS)highText.SetActive(true);   // �X�R�A�X�V�e�L�X�g��\��
    }

    private void BackTitle()
    {
        BGMSource.instance.StopBGM();
        SceneManager.LoadScene("Title");
    }
}
