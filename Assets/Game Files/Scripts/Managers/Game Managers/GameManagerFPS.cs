using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerFPS : GameManager
{
    [Header("ゴールの鍵出現に必要なエサの数"), SerializeField]
    private int needBaitNum = 0;

    [Header("ゴールの鍵の画像"), SerializeField]
    private GameObject goalKeyImg;

    private PlayerBaseFPS playerFPS;
    private GoalKeySpawner goalKeySpawner;

    new void Awake()
    {
        base.Awake();

        playerFPS = FindAnyObjectByType<PlayerBaseFPS>();
        goalKeySpawner = FindAnyObjectByType<GoalKeySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        DispLife();
        DispGoalKeyImg();

        if (EatBaitCount >= needBaitNum) goalKeySpawner.SpawnGoalKey();
    }

    private void DispGoalKeyImg()
    {
        goalKeyImg.SetActive(playerFPS.HasKey);
    }

    public new void GameOver()   // ゲームオーバー処理
    {
        base.GameOver();

        ScoreRanking.instance.RankInFPS(ScoreManager.scoreNow);

        BGMSource.instance.StopBGM();
    }
}
