using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItemSpowner : MonoBehaviour
{
    [Header("生成に必要なエサの数"), SerializeField]
    private int[] needBaitCount;

    [Header("生成するアイテム"), SerializeField]
    private GameObject[] bonusItems;

    private bool spawnTrigger = true;     // 二回目用生成トリガー
    private bool isSecondSpown = false;   // 二回目の生成か

    private GameManagerTPS gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManagerTPS>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSecondSpown)   // 最初の生成の場合
        {
            if (gameManager.EatBaitCount >= needBaitCount[0])   // 食べたエサ数が設定数を越えた場合
            {
                Instantiate(ItemSelector(), transform.position, Quaternion.identity);
                isSecondSpown = true;
            }
        }
        else   // 二回目の生成の場合
        {
            if (gameManager.EatBaitCount >= needBaitCount[1])   // 食べたエサ数が設定数を越えた場合
            {
                if (spawnTrigger)
                {
                    Instantiate(ItemSelector(), transform.position, Quaternion.identity);
                    spawnTrigger = false;
                }
            }
        }
    }

    private GameObject ItemSelector()   // 現在のステージに応じて生成するアイテムを変更
    {
        int num = 0;

        if (GameManagerTPS.stageNum == 1) num = 0;                                  // さくらんぼ
        if (GameManagerTPS.stageNum == 2) num = 1;                                  // 苺
        if (GameManagerTPS.stageNum == 3 || GameManagerTPS.stageNum == 4) num = 2;     // オレンジ
        if (GameManagerTPS.stageNum == 5 || GameManagerTPS.stageNum == 6) num = 3;     // りんご
        if (GameManagerTPS.stageNum == 7 || GameManagerTPS.stageNum == 8) num = 4;     // メロン
        if (GameManagerTPS.stageNum == 9 || GameManagerTPS.stageNum == 10) num = 5;    // ボス・ギャラクシアン
        if (GameManagerTPS.stageNum == 11 || GameManagerTPS.stageNum == 12) num = 6;   // ベル
        if (GameManagerTPS.stageNum >= 13) num = 7;                                 // 鍵

        return bonusItems[num];
    }
}
