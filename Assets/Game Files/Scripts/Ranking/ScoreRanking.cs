using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRanking : MonoBehaviour
{
    public static ScoreRanking instance;

    private int rank1TPS, rank2TPS, rank3TPS = 0;   // TPSモードランキング変数
    private int rank1FPS, rank2FPS, rank3FPS = 0;   // FPSモードランキング変数

    [Header("ランキング表示テキスト"), SerializeField]
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

    private void GetRanking()   // ランキングの取得
    {
        // TPSモードのランキング
        rank1TPS = PlayerPrefs.GetInt("Rank1TPS");
        rank2TPS = PlayerPrefs.GetInt("Rank2TPS");
        rank3TPS = PlayerPrefs.GetInt("Rank3TPS");

        // FPSモードのランキング
        rank1FPS = PlayerPrefs.GetInt("Rank1FPS");
        rank2FPS = PlayerPrefs.GetInt("Rank2FPS");
        rank3FPS = PlayerPrefs.GetInt("Rank3FPS");
    }

    public void DispRanking(Text[] _rankTextTPS, Text[] _rankTextFPS)   // ランキング表示
    {
        _rankTextTPS[0].text = rank1TPS.ToString();
        _rankTextTPS[1].text = rank2TPS.ToString();
        _rankTextTPS[2].text = rank3TPS.ToString();

        _rankTextFPS[0].text = rank1FPS.ToString();
        _rankTextFPS[1].text = rank2FPS.ToString();
        _rankTextFPS[2].text = rank3FPS.ToString();
    }

    public void RankInTPS(int _score)   // ランキング書き換え
    {
        if (_score >= rank1TPS)   // スコアがランク１位以上の場合
        {
            rank3TPS = rank2TPS;
            rank2TPS = rank1TPS;
            rank1TPS = _score;

            PlayerPrefs.SetInt("Rank3TPS", rank3TPS);
            PlayerPrefs.SetInt("Rank2TPS", rank2TPS);
            PlayerPrefs.SetInt("Rank1TPS", rank1TPS);
        }
        else if (_score >= rank2TPS)   // スコアがランク２位以上１位未満の場合
        {
            rank3TPS = rank2TPS;
            rank2TPS = _score;

            PlayerPrefs.SetInt("Rank3TPS", rank3TPS);
            PlayerPrefs.SetInt("Rank2TPS", rank2TPS);
        }
        else if (_score >= rank3TPS)   // スコアがランク３位以上２位未満の場合
        {
            rank3TPS = _score;

            PlayerPrefs.SetInt("Rank3TPS", rank3TPS);
        }

        //DispRanking();
    }

    public void RankInFPS(int _score)   // ランキング書き換え
    {
        if (_score >= rank1FPS)   // スコアがランク１位以上の場合
        {
            rank3FPS = rank2FPS;
            rank2FPS = rank1FPS;
            rank1FPS = _score;

            PlayerPrefs.SetInt("Rank3FPS", rank3FPS);
            PlayerPrefs.SetInt("Rank2FPS", rank2FPS);
            PlayerPrefs.SetInt("Rank1FPS", rank1FPS);
        }
        else if (_score >= rank2FPS)   // スコアがランク２位以上１位未満の場合
        {
            rank3FPS = rank2FPS;
            rank2FPS = _score;

            PlayerPrefs.SetInt("Rank3FPS", rank3FPS);
            PlayerPrefs.SetInt("Rank2FPS", rank2FPS);
        }
        else if (_score >= rank3FPS)   // スコアがランク３位以上２位未満の場合
        {
            rank3FPS = _score;

            PlayerPrefs.SetInt("Rank3FPS", rank3FPS);
        }

        //DispRanking();
    }

    public void DeleteRanking()   // ランキングの削除
    {
        // 保存内容の削除
        PlayerPrefs.DeleteKey("Rank1TPS");
        PlayerPrefs.DeleteKey("Rank2TPS");
        PlayerPrefs.DeleteKey("Rank3TPS");
        PlayerPrefs.DeleteKey("Rank1FPS");
        PlayerPrefs.DeleteKey("Rank2FPS");
        PlayerPrefs.DeleteKey("Rank3FPS");

        // ランキングリセット
        rank1TPS = 0;
        rank2TPS = 0;
        rank3TPS = 0;

        rank1FPS = 0;
        rank2FPS = 0;
        rank3FPS = 0;

        //DispRanking();
    }
}
