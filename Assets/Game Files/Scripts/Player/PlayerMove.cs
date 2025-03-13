using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動速度"), SerializeField]
    private float speed = 0;

    // 現在の移動速度
    private float moveX = 0;
    private float moveZ = 0;

    private float playerAngle = 0;   // プレイヤーの向き

    [Header("上側の当たり判定"), SerializeField] private WallHitCollision colliderFront;
    [Header("下側の当たり判定"), SerializeField] private WallHitCollision colliderBack;
    [Header("左側の当たり判定"), SerializeField] private WallHitCollision colliderLeft;
    [Header("右側の当たり判定"), SerializeField] private WallHitCollision colliderRight;

    public enum MoveState   // 移動状態
    {
        front,   // 前進
        back,    // 後退
        left,    // 左
        right,   // 右
        stop,    // 停止
        none     // なし
    }
    public MoveState moveState;       // 現在移動している方向
    public MoveState moveStateSave;   // どの方向へ移動するかを保存する

    private Rigidbody rb;

    private PlayerBase player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = GetComponent<PlayerBase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveState = MoveState.stop;
        moveStateSave = MoveState.none;
        UpdateParam();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsDead)
        {
            moveStateSave = MoveState.none;
            moveState = MoveState.stop;
            UpdateParam();

            return;
        }

        InputMove();
        ChangeMoveState();

        // 速度と方向の適用
        rb.velocity = new Vector3(moveX, 0, moveZ);
    }

    private void InputMove()   // 入力
    {
        // 入力受け取り
        if (Input.GetKey(KeyCode.W))
        {
            moveStateSave = MoveState.front;   // 移動方向を保存
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveStateSave = MoveState.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveStateSave = MoveState.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveStateSave = MoveState.right;
        }
    }

    private bool IsWall(WallHitCollision anyCollider)   // 受け取ったコライダーに壁が触れているかチェック
    {
        return anyCollider.IsHit;
    }

    private void ChangeMoveState()
    {
        // 保存した移動方向に壁がない場合
        if (moveStateSave == MoveState.front && !IsWall(colliderFront))
        {
            moveState = MoveState.front;   // 移動方向を変更
            UpdateParam();                 // 各パラメーターを更新
        }
        if (moveStateSave == MoveState.back && !IsWall(colliderBack))
        {
            moveState = MoveState.back;
            UpdateParam();
        }
        if (moveStateSave == MoveState.left && !IsWall(colliderLeft))
        {
            moveState = MoveState.left;
            UpdateParam();
        }
        if (moveStateSave == MoveState.right && !IsWall(colliderRight))
        {
            moveState = MoveState.right;
            UpdateParam();
        }
    }

    private void UpdateParam()   // 移動に関する値の更新
    {
        var oldAngle = playerAngle;   // 更新前の向きを保存

        switch (moveState)
        {
            // 移動状態に応じて速度と向きを変更
            case MoveState.front:   // 前進中の場合
                moveX = 0;
                moveZ = speed;
                playerAngle = 0;
                break;

            case MoveState.back:    // 後退中の場合
                moveX = 0;
                moveZ = -speed;
                playerAngle = 180;
                break;

            case MoveState.left:    // 左移動中の場合
                moveX = -speed;
                moveZ = 0;
                playerAngle = 270;
                break;

            case MoveState.right:   // 右移動中の場合
                moveX = speed;
                moveZ = 0;
                playerAngle = 90;
                break;

            case MoveState.stop:    // 停止中の場合
                moveX = 0;
                moveZ = 0;
                break;
        }

        if (player.IsDead) return;
        if (oldAngle == playerAngle) return;   // 向きが変わっていなかった場合終了

        transform.DOLocalRotate(new Vector3(0, playerAngle, 0), 0.1f);   // プレイヤーの回転
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 特定のタイミングで停止状態に
        if (IsWall(colliderFront) && moveState == MoveState.front) moveState = MoveState.stop;
        if (IsWall(colliderBack)  && moveState == MoveState.back)  moveState = MoveState.stop;
        if (IsWall(colliderLeft)  && moveState == MoveState.left)  moveState = MoveState.stop;
        if (IsWall(colliderRight) && moveState == MoveState.right) moveState = MoveState.stop;
    }

    public float Speed
    {
        get { return speed; }
    }

    private void OnDestroy()
    {
        DOTween.Clear(true);
    }
}
