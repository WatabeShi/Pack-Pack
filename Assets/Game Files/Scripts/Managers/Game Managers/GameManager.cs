using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("ゲームモード切り替え"), SerializeField]
    protected bool isFPS = false;

    protected int baitCount = 0;      // ステージ内のエサの総数
    protected int eatBaitCount = 0;   // 食べたエサの数

    //[Header("残機表示画像"), SerializeField]
    //private GameObject[] lifeImgs;

    protected LifeManager lifeManager;

    protected PowerUpBait[] puBaits;
    protected EnemyBase[] enemies;

    protected void Awake()
    {
        lifeManager = FindAnyObjectByType<LifeManager>();

        puBaits = FindObjectsByType<PowerUpBait>(FindObjectsSortMode.None);
        enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
    }

    // Start is called before the first frame update
    protected void Start()
    {
        Time.timeScale = 1;

        baitCount = FindObjectsByType<BaitBase>(FindObjectsSortMode.None).Length;   // エサの総数を記録
    }

    public void IncreaseEatCount()   // 食べたエサの数の加算
    {
        eatBaitCount++;
    }

    protected void DispLife()   // ライフの表示
    {
        lifeManager.DispLife();
    }

    public void GameOver()   // ゲームオーバー処理
    {
        SceneManager.LoadScene("Gameover");
    }

    public int EatBaitCount
    {
        get { return eatBaitCount; }
    }

    public bool IsFPS
    {
        get { return isFPS; }
    }
}
