using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    // プレイヤーのオブジェクト
    [Header("ターゲットとなるオブジェクト"), SerializeField]
    private GameObject targetObj;

    // カメラのプレイヤーの距離
    private Vector3 offset;

    private GameManager gameManager;

    private PlayerBase player;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        player = FindAnyObjectByType<PlayerBase>();

        offset = transform.position - targetObj.transform.position;
    }

    void Update()
    {
        if (player.IsDead) return;

        transform.position = targetObj.transform.position + offset;
    }

    public void UpdateTarget()   // ターゲットのアップデート
    {
        player = FindAnyObjectByType<PlayerBase>();
        targetObj = player.gameObject;

        transform.position = targetObj.transform.position + offset;
    }
}
