using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRanking : MonoBehaviour
{
    public static ScoreRanking instance;

    private int rank1TPS, rank2TPS, rank3TPS = 0;   // TPS���[�h�����L���O�ϐ�
    private int rank1FPS, rank2FPS, rank3FPS = 0;   // FPS���[�h�����L���O�ϐ�

    [Header("�����L���O�\���e�L�X�g"), SerializeField]
    private Text[] rankText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetRanking();
        //DispRanking();
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) DeleteRanking();
    }

#endif

    private void GetRanking()   // �����L���O�̎擾
    {
        // TPS���[�h�̃����L���O
        rank1TPS = PlayerPrefs.GetInt("Rank1TPS");
        rank2TPS = PlayerPrefs.GetInt("Rank2TPS");
        rank3TPS = PlayerPrefs.GetInt("Rank3TPS");

        // FPS���[�h�̃����L���O
        rank1FPS = PlayerPrefs.GetInt("Rank1FPS");
        rank2FPS = PlayerPrefs.GetInt("Rank2FPS");
        rank3FPS = PlayerPrefs.GetInt("Rank3FPS");
    }

    public void DispRanking(Text[] _rankTextTPS, Text[] _rankTextFPS)   // �����L���O�\��
    {
        _rankTextTPS[0].text = rank1TPS.ToString();
        _rankTextTPS[1].text = rank2TPS.ToString();
        _rankTextTPS[2].text = rank3TPS.ToString();

        _rankTextFPS[0].text = rank1FPS.ToString();
        _rankTextFPS[1].text = rank2FPS.ToString();
        _rankTextFPS[2].text = rank3FPS.ToString();
    }

    public void RankInTPS(int _score)   // �����L���O��������
    {
        if (_score >= rank1TPS)   // �X�R�A�������N�P�ʈȏ�̏ꍇ
        {
            rank3TPS = rank2TPS;
            rank2TPS = rank1TPS;
            rank1TPS = _score;

            PlayerPrefs.SetInt("Rank3TPS", rank3TPS);
            PlayerPrefs.SetInt("Rank2TPS", rank2TPS);
            PlayerPrefs.SetInt("Rank1TPS", rank1TPS);
        }
        else if (_score >= rank2TPS)   // �X�R�A�������N�Q�ʈȏ�P�ʖ����̏ꍇ
        {
            rank3TPS = rank2TPS;
            rank2TPS = _score;

            PlayerPrefs.SetInt("Rank3TPS", rank3TPS);
            PlayerPrefs.SetInt("Rank2TPS", rank2TPS);
        }
        else if (_score >= rank3TPS)   // �X�R�A�������N�R�ʈȏ�Q�ʖ����̏ꍇ
        {
            rank3TPS = _score;

            PlayerPrefs.SetInt("Rank3TPS", rank3TPS);
        }

        //DispRanking();
    }

    public void RankInFPS(int _score)   // �����L���O��������
    {
        if (_score >= rank1FPS)   // �X�R�A�������N�P�ʈȏ�̏ꍇ
        {
            rank3FPS = rank2FPS;
            rank2FPS = rank1FPS;
            rank1FPS = _score;

            PlayerPrefs.SetInt("Rank3FPS", rank3FPS);
            PlayerPrefs.SetInt("Rank2FPS", rank2FPS);
            PlayerPrefs.SetInt("Rank1FPS", rank1FPS);
        }
        else if (_score >= rank2FPS)   // �X�R�A�������N�Q�ʈȏ�P�ʖ����̏ꍇ
        {
            rank3FPS = rank2FPS;
            rank2FPS = _score;

            PlayerPrefs.SetInt("Rank3FPS", rank3FPS);
            PlayerPrefs.SetInt("Rank2FPS", rank2FPS);
        }
        else if (_score >= rank3FPS)   // �X�R�A�������N�R�ʈȏ�Q�ʖ����̏ꍇ
        {
            rank3FPS = _score;

            PlayerPrefs.SetInt("Rank3FPS", rank3FPS);
        }

        //DispRanking();
    }

    public void DeleteRanking()   // �����L���O�̍폜
    {
        // �ۑ����e�̍폜
        PlayerPrefs.DeleteKey("Rank1TPS");
        PlayerPrefs.DeleteKey("Rank2TPS");
        PlayerPrefs.DeleteKey("Rank3TPS");
        PlayerPrefs.DeleteKey("Rank1FPS");
        PlayerPrefs.DeleteKey("Rank2FPS");
        PlayerPrefs.DeleteKey("Rank3FPS");

        // �����L���O���Z�b�g
        rank1TPS = 0;
        rank2TPS = 0;
        rank3TPS = 0;

        rank1FPS = 0;
        rank2FPS = 0;
        rank3FPS = 0;

        //DispRanking();
    }
}
