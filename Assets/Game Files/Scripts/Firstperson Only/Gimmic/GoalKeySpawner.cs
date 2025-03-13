using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeySpawner : MonoBehaviour
{
    [Header("生成するゴールの鍵"), SerializeField]
    private GameObject goalKey;

    [Header("生成する場所候補のまとまり"), SerializeField]
    private GameObject spawnGroup;
    private Transform[] spawnPoints;

    private bool spawnTrigger = true;   // 生成用トリガー

    private void Awake()
    {
        GetSpawnPoint();   // 鍵の生成地点の取得
    }

    public void SpawnGoalKey()   // 鍵の生成処理
    {
        if (!spawnTrigger) return;

        int pointNum = Random.Range(0, spawnPoints.Length);
        var key = Instantiate(goalKey, spawnPoints[pointNum].position, goalKey.transform.rotation);
        key.transform.SetParent(transform);
        spawnTrigger = false;   // トリガー無効化
    }

    private void GetSpawnPoint()
    {
        spawnPoints = new Transform[spawnGroup.transform.childCount];   // 配列の定義

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = spawnGroup.transform.GetChild(i).transform;   // 子オブジェクトを配列へ
        }
    }
}
