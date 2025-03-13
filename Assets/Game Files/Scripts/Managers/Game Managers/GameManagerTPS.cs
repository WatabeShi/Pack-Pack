using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTPS : GameManager
{
    private bool isClear = false;   // ステージクリアフラグ

    //private bool playerDead = false;

    private int stageNowNum = 1;      // 開始ステージをインスペクター上で設定する用変数
    public static int stageNum = 1;   // 現在のステージ

    private ObjectMover[] objMover;
    private PlayerBase player;

    new void Awake()
    {
        base.Awake();

        objMover = FindObjectsByType<ObjectMover>(FindObjectsSortMode.None);
        player = FindAnyObjectByType<PlayerBase>();
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        isClear = false;
        stageNum = stageNowNum;
    }

    // Update is called once per frame
    void Update()
    {
        DispLife();   // ライフの表示

        // 全てのエサを食べたらクリア
        if (eatBaitCount == baitCount) isClear = true;
    }

    private void StartClearProcess()   // クリア時に一度呼ばれる処理
    {
        int delayTime = 1;
        StartCoroutine(ClearProcess(delayTime));
    }

    private IEnumerator ClearProcess(float delay)   // クリア時の処理
    {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1;

        if(IsCoffeeBreakStage())   // コーヒーブレイクの必要なステージか
        {
            int breakTime = 1;   // アニメーション再生時間
            yield return StartCoroutine(CoffeeBreak(breakTime));   // コーヒーブレイクアニメーションを再生＆再生終了まで次の処理を待機
        }

        stageNum++;   // ステージ数の加算
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   // ステージ再構築
    }

    private bool IsCoffeeBreakStage()   // コーヒーブレイクの必要なステージか
    {
        if(stageNum == 2 || stageNum == 5 || stageNum == 9 || stageNum == 13 || stageNum == 17)   // 特定のステージの場合
        {
            return true;   // コーヒーブレイクを有効化
        }
        else
        {
            return false;   // コーヒーブレイクを無効化
        }
    }

    private IEnumerator CoffeeBreak(float delay)   // コーヒーブレイクの処理
    {
        if(stageNum == 2)
        {
            print("ステージ２コーヒーブレイク中だお");
        }
        else if(stageNum == 5)
        {
            print("ステージ５コーヒーブレイク中だお");
        }
        else
        {
            print("ステージ？コーヒーブレイク中だお");
        }

        yield return new WaitForSeconds(delay);
    }

    public void StartPlayerDeadProcess()   // プレイヤー死亡時の処理を開始させる
    {
        // 死亡フラグが有効の場合実行しない
        //if (player.IsDead) return;

        int delayTime = 3;   // 死亡アニメーション再生時間
        StartCoroutine(PlayerDeadProcess(delayTime));
    }

    private IEnumerator PlayerDeadProcess(float delay)   // プレイヤー死亡時の処理
    {
        //playerDead = true;   // 死亡フラグ有効化
        //lifeCount--;         // 残機を減らす
        lifeManager.Damage();
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1;

        if(!lifeManager.IsGameOver())
        {
            // カメラ、敵、エサのターゲット再設定
            foreach(ObjectMover _objMover in objMover)
            {
                _objMover.UpdateTarget();
            }
            foreach(EnemyBase enemy in enemies)
            {
                enemy.Respawn(false);
            }
            foreach(PowerUpBait puBait in puBaits)
            {
                puBait.UpdateTarget();
            }

            player.Respawn();   // プレイヤーのリスポーン

            //playerDead = false;   // 死亡フラグ無効化
        }
        else   // 残機がない時
        {
            GameOver();   // ゲームオーバー処理
        }
    }

    private void OnEnable()
    {
        EventManager.clearEvent += StartClearProcess;         // クリア処理をイベントに代入
        //EventManager.gameoverEvent += GameoverProcess;   // ゲームオーバー処理をイベントに代入
    }

    private void OnDisable()
    {
        EventManager.clearEvent -= StartClearProcess;      // クリア処理をイベントから除外
        //EventManager.GameoverEvent -= GameoverProcess;   // ゲームオーバー処理をイベントから除外
    }

    //public bool PlayerDead
    //{
    //    get { return playerDead; }
    //}

    public new void GameOver()   // ゲームオーバー処理
    {
        base.GameOver();

        ScoreRanking.instance.RankInTPS(ScoreManager.scoreNow);

        //BGMSource.instance.StopBGM();
    }

    public bool IsClear
    {
        get { return isClear; }
    }
}
