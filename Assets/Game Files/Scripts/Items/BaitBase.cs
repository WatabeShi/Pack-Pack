using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitBase : MonoBehaviour
{
    [Header("得られるスコア"), SerializeField]
    protected int score = 10;

    [Header("食べた時のSE"), SerializeField]
    private AudioClip eatSE;

    protected GameManager gameManager;
    protected ScoreManager scoreManager;

    protected bool isFPS = false;

    protected void Awake()
    {
        gameManager = /*isFPS ? FindAnyObjectByType<GameManagerFPS>() : */FindAnyObjectByType<GameManager>();
        isFPS = gameManager.IsFPS;

        scoreManager = FindAnyObjectByType<ScoreManager>();

        if (isFPS) GetComponent<SphereCollider>().radius = 0.02f;   // FPSモード時に当たり判定のサイズ調整
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            BaseProcess(true);
        }
    }

    protected void BaseProcess(bool countTrigger)   // プレイヤーと触れた時の基本処理
    {
        if (scoreManager != null) scoreManager.AddScore(score);   // スコアを追加
        if(countTrigger) gameManager.IncreaseEatCount();          // トリガーが有効の場合食べた数を増やす

        SESource.instance.PlaySE(eatSE);

        Destroy(this.gameObject);   // 自分を削除
    }
}
