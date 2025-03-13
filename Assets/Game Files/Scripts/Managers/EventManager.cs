using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action clearEvent;   // クリアイベント
    private bool clearEventTrigger = true;   // クリアイベント有効フラグ

    //public static event Action gameoverEvent;   // ゲームオーバーイベント
    //private bool gameoverEventTrigger = true;   // ゲームオーバーイベント有効フラグ

    private GameManagerTPS gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManagerTPS>();
    }

    // Update is called once per frame
    void Update()
    {
        ClearEventExecuter();
        //GameoverEventExecuter();
    }

    private void ClearEventExecuter()   // クリアイベント実行者
    {
        if (!clearEventTrigger) return;     // イベントフラグが無効の場合
        if (!gameManager.IsClear) return;   // クリアフラグが無効の場合

        clearEvent?.Invoke();        // イベント実行
        clearEventTrigger = false;   // フラグ無効化
    }

    //private void GameoverEventExecuter()   // ゲームオーバーイベント実行者
    //{
    //    if (!gameoverEventTrigger) return;     // イベントフラグが無効の場合
    //    if (!gameManager.IsGameOver) return;   // ゲームオーバーフラグが無効の場合

    //    gameoverEvent?.Invoke();        // イベント実行
    //    gameoverEventTrigger = false;   // フラグ無効化
    //}
}
