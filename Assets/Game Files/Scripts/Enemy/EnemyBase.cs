using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public enum MovePattern   // 行動パターン
    {
        roaming,   // 縄張り徘徊
        chase,     // プレイヤー追跡
        //izike      // いじけ
    }
    public MovePattern movePattern;

    private bool isIzike = false;   // いじけ状態フラグ

    private float speedNormal = 0f;   // 通常速度
    private float speedIzike = 0;       // いじけ状態の速度
    private float speedNow = 0;         // 現在の速度

    private Vector3 targetPos = Vector3.zero;

    [Header("行動パターンが切り替わるのにかかる時間"), SerializeField]
    private float patternChangeTime = 0;
    private float patternChangeTimer = 0;   // 切り替え時間カウント用変数
    private float izikeTimer = 0;           // いじけ状態のタイマー用変数

    [Header("自分の目"), SerializeField]
    private GameObject[] eyes;

    private bool isDead = false;

    [Header("スポーンしてから動きだすまでの待機時間"), SerializeField]
    private float moveDelay = 0;
    private bool isStartMove = false;

    [Header("リスポーンにかかる時間"), SerializeField]
    private float respawnTime;
    [Header("リスポーンする地点"), SerializeField]
    private Transform respawnPoint;
    private Vector3 startPosition;

    [Header("自分の縄張り"), SerializeField]
    private TerritoryChecker territory;
    private Transform[] movePoints;

    private Color defaultColor;   // 元の色保存変数

    [Header("プレイヤー"), SerializeField]
    protected PlayerBase player;

    [Header("メッシュの場所"), SerializeField]
    private Renderer _renderer;
    private Collider myCollider;
    //private Rigidbody rb;
    private NavMeshAgent agent;

    protected GameManager gameManager;

    private void Awake()
    {
        territory = territory.GetComponent<TerritoryChecker>();

        myCollider = GetComponent<Collider>();
        //rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        //myRenderer = GetComponent<Renderer>();

        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;   // ゲーム開始時の位置を保存

        GetMovePoints();

        movePattern = MovePattern.roaming;   // 最初の行動パターンを設定

        speedNormal = FindAnyObjectByType<PlayerMove>().Speed * 0.8f;   // 通常移動速度をプレイヤーより少し遅く設定
        speedIzike = speedNormal / 2;   // いじけ状態の移動速度の計算
        speedNow = speedNormal;         // 速度を適用

        defaultColor = _renderer.material.color;   // デフォルトの色の保存

        StartCoroutine(StartMove());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();   // 速度の変更と適用

        if (isDead) return;

        ChangeTimer();
        IzikeChanger();
        SetMovePos();
    }

    private void GetMovePoints()   // 縄張りの子オブジェクトを取得する
    {
        var children = territory.gameObject.transform.childCount;
        movePoints = new Transform[children];

        for(int i = 0; i < children; i++)
        {
            movePoints[i] = territory.transform.GetChild(i);
        }
    }

    private void SetMovePos()   // 移動地点の設定
    {
        if (agent.enabled == true) agent.destination = targetPos;
    }

    private IEnumerator StartMove()   // スポーンから巣を出るまで待機時間を設ける
    {
        isStartMove = false;
        targetPos = transform.position;

        yield return new WaitForSeconds(moveDelay);

        isStartMove = true;

        UpdateMovePoint(true);
    }

    private void UpdateMovePoint(bool isStart)   // 縄張り内における移動地点の変更
    {
        // 目的地とエージェントの距離が0.5以内であり初動でない場合先の処理へ
        if (agent.remainingDistance > 0.5f && !isStart) return;

        // ランダムで移動する場所を決定
        int pointNum = Random.Range(0, movePoints.Length);
        var nextPoint = movePoints[pointNum].position;

        targetPos = nextPoint;
    }

    private void ChangePattern()   // 行動パターンを変更
    {
        if (movePattern == MovePattern.roaming)
        {
            movePattern = MovePattern.chase;
        }
        else if (movePattern == MovePattern.chase)
        {
            movePattern = MovePattern.roaming;
        }
    }

    private void ChangeTimer()
    {
        // いじけ状態でない場合
        if (isIzike) return;

        if (patternChangeTimer <= patternChangeTime)   // 指定した時間経過するまで
        {
            patternChangeTimer += Time.deltaTime;
        }
        else   // 指定した時間経過したら
        {
            ChangePattern();
            patternChangeTimer = 0;
        }
    }

    private void UpdateState()
    {
        if (isDead)
        {
            speedNow = 0;
            return;
        }

        if (isIzike)   // いじけ状態の場合
        {
            speedNow = speedIzike;
            movePattern = MovePattern.roaming;
            _renderer.material.color = new Vector4(0, 0, 1, 0);
        }
        else
        {
            speedNow = speedNormal;
            _renderer.material.color = defaultColor;
        }

        agent.speed = speedNow;

        if (!isStartMove) return;

        if (movePattern == MovePattern.roaming)   // 徘徊中の場合
        {
            UpdateMovePoint(false);

            if (territory.IsInArea) targetPos = player.transform.position;
        }
        else   // 追跡中の場合
        {
            targetPos = player.transform.position;
        }
    }

    public void StartIzike()
    {
        izikeTimer = player.PowerUpTime;   // タイマーを設定

        // ランダムで移動する場所を決定
        int pointNum = Random.Range(0, movePoints.Length);
        var nextPoint = movePoints[pointNum].position;

        targetPos = nextPoint;
    }

    private void IzikeChanger()   // いじけ状態の切り替え
    {
        if(izikeTimer > 0)
        {
            isIzike = true;
            izikeTimer -= Time.deltaTime;   // タイムを減らす
        }
        else
        {
            isIzike = false;
        }
    }

    private IEnumerator Dead()
    {
        isDead = true;

        izikeTimer = 0;   // いじけ状態を解除

        SomethingActivator(false);

        //isStartMove = false;

        yield return new WaitForSeconds(respawnTime);

        SomethingActivator(true);

        Respawn(true);
    }

    private void SomethingActivator(bool _isActive)
    {
        _renderer.enabled = _isActive;
        myCollider.enabled = _isActive;
        //rb.isKinematic = _isActive;
        agent.enabled = _isActive;

        foreach (var eye in eyes) eye.SetActive(_isActive);
    }

    public void Respawn(bool isDeadMe)
    {
        var warpPos = (isDeadMe) ? respawnPoint.position : startPosition;

        //this.transform.position = respawnPoint.position;
        agent.Warp(warpPos);

        patternChangeTimer = 0;

        movePattern = MovePattern.roaming;   // 徘徊状態へ戻す

        isDead = false;

        targetPos = transform.position;

        StartCoroutine(StartMove());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;   // 触れたものがプレイヤーでない場合終了

        if (isIzike) StartCoroutine(Dead());   // 自分の死亡処理
        else         player.Dead();            // プレイヤーの死亡処理
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    //public void UpdateTarget()   // プレイヤー再検索（プレイヤーが復活した時用）
    //{
    //    player = FindAnyObjectByType<PlayerBase>();
    //}

    //public void StartDead()
    //{
    //    StartCoroutine(Dead());
    //}
}
