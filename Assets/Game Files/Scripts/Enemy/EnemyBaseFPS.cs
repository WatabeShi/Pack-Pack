using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Cinemachine;

public class EnemyBaseFPS : MonoBehaviour
{
    [Header("最大HP"), SerializeField]
    private int hp = 50;
    private int hpMax;   // 最大HP保存用変数

    [Header("移動速度"), SerializeField]
    private float speedMove = 5;

    [Header("移動範囲（X軸）"), SerializeField]
    private float rangeX;
    [Header("移動範囲（Z軸）"), SerializeField]
    private float rangeZ;
    Vector3 movePos = Vector3.zero;

    [Header("プレイヤー"), SerializeField]
    private Transform player;

    [Header("プレイヤーを追跡しだす距離"), SerializeField]
    private float chaseDistance = 0;

    [Header("復活にかかる時間"), SerializeField]
    private float respawnTime = 0;

    [Header("死亡時のエフェクト"), SerializeField]
    private GameObject deadEffect;
    [Header("死亡SE"), SerializeField]
    private AudioClip deadSE;

    [Header("復活地点のまとまり"), SerializeField]
    private GameObject rsPointGroup;
    private TerritoryChecker[] respawnPoints;    // 全てのリスポーン地点
    private TerritoryChecker[] _respawnPoints;   // プレイヤーが近くにいない場所
    private int pointCount = 0;                  // リスポーンできる地点の数

    [Header("倒された時に加算されるスコア"), SerializeField]
    private int killScore;

    [Header("お目目"), SerializeField]
    private GameObject[] eyes;

    [Header("ミニマップ用アイコン"), SerializeField]
    private GameObject minimapIcon;

    private NavMeshAgent agent;

    private Renderer[] _renderers;
    private Collider myCollider;
    private AudioSource audioSource;
    private CinemachineCollisionImpulseSource impSource;

    private ScoreManager scoreManager;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();

        agent = GetComponent<NavMeshAgent>();
        myCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        impSource = GetComponent<CinemachineCollisionImpulseSource>();

        scoreManager = FindAnyObjectByType<ScoreManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetStartRsPoint();
        ChangeMovePos();

        hpMax = hp;   // 最大HPの保存
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = IsDead() ? 0 : speedMove;   // 状態に応じて速度を変更

        ChangeDestination();
        LookPlayer();
    }

    private void GetStartRsPoint()   // リスポーン地点の取得（ゲーム開始時用）
    {
        pointCount = rsPointGroup.transform.childCount;
        respawnPoints = new TerritoryChecker[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            respawnPoints[i] = rsPointGroup.transform.GetChild(i).gameObject.GetComponent<TerritoryChecker>();
        }
    }

    private void GetRsPoint()   // リスポーン地点の取得（死亡時用）
    {
        _respawnPoints = null;   // 初期化
        _respawnPoints = respawnPoints.Where(e => !e.IsInArea).ToArray();   // 一定の範囲内にプレイヤーがいない場所を配列に追加

        pointCount = _respawnPoints.Length;   // リスポーンできる地点の数の更新
    }

    private void ChangeMovePos()   // 移動地点の設定
    {
        // 指定した範囲内でランダムで決定
        var moveX = Random.Range(-rangeX, rangeX);
        var moveZ = Random.Range(-rangeZ, rangeZ);
        movePos = new Vector3(moveX, transform.position.y, moveZ);
    }

    private void ChangeDestination()   // 移動地点の更新
    {
        if (IsDead()) return;

        if (agent.remainingDistance < 1) ChangeMovePos();   // 目標地点との距離が１以内だったら

        agent.destination = IsNearPlayer() ? player.position : movePos;   // プレイヤーが近くにいるかで移動地点を変更
    }

    private bool IsNearPlayer()   // プレイヤーが周辺にいるか判定
    {
        var playerVec = player.position - this.transform.position;   // プレイヤーと自分のベクトル計算

        if (playerVec.magnitude <= chaseDistance) return true;   // 指定した距離内にプレイヤーがいる場合 true
        else return false;
    }

    public void Damage(int damage)   // ダメージを受ける処理
    {
        if (IsDead()) return;   // 死んでいたら先の処理を実行しない

        hp -= damage;            // HPを減らす
        hp = Mathf.Max(hp, 0);   // 値の調整

        if (IsDead()) Dead();   // 死亡関数
    }

    private bool IsDead()   // 死亡判定
    {
        return (hp <= 0) ? true : false;
    }

    private void Dead()
    {
        if (scoreManager != null) scoreManager.AddScore(killScore);   // スコアの加算

        minimapIcon.SetActive(false);   // ミニマップアイコンを消す
        foreach (GameObject eye in eyes) eye.SetActive(false);   // 目の非表示

        // レンダラーとコライダー無効化
        foreach (Renderer ren in _renderers) ren.enabled = false;
        myCollider.enabled = false;

        var _deadEffect = Instantiate(deadEffect, this.transform.position, Quaternion.identity);   // エフェクト生成
        _deadEffect.transform.localScale = new Vector3(2, 2, 2);   // エフェクトのサイズ調整    
        audioSource.PlayOneShot(deadSE);   // SE再生

        var dir = this.transform.position - player.position;

        impSource.GenerateImpulse();
        //Vector3 shakePow = new Vector3(5, 5, 5);
        //Camera.main.transform.DOShakeRotation(0.5f, shakePow).SetRelative();   // カメラの揺れ

        StartCoroutine(Respawn());   // 復活コルーチン起動
    }

    private IEnumerator Respawn()   // 復活
    {
        yield return new WaitForSeconds(respawnTime);   // 指定した時間分待機

        // レンダラーとコライダー有効化
        myCollider.enabled = true;
        foreach (Renderer ren in _renderers) ren.enabled = true;

        foreach (GameObject eye in eyes) eye.SetActive(true);   // 目の表示
        minimapIcon.SetActive(true);   // ミニマップアイコンを表示

        // 転送処理
        GetRsPoint();   // 転送できる場所の取得
        int rsPointNum = Random.Range(0, pointCount);   // 取得した場所の中からランダムで場所を決定
        this.transform.position = _respawnPoints[rsPointNum].gameObject.transform.position;   // 決定した場所に転送

        hp = hpMax;   // HP完全回復
    }

    private void LookPlayer()   // 目が常にプレイヤーを見続ける処理
    {
        if (player == null || eyes == null) return;

        foreach (GameObject eye in eyes) eye.transform.LookAt(player);

        //var angleX = 0f;
        //var angleY = 0f;
        //foreach (GameObject eye in eyes) angleX = Mathf.Clamp(eye.transform.rotation.x, -40, 30);
        //foreach (GameObject eye in eyes) angleY = Mathf.Clamp(eye.transform.rotation.y, -35, 32);

        //foreach (GameObject eye in eyes) eye.transform.rotation = Quaternion.Euler(angleX, angleY, 0);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();   // 全てのコルーチンを停止
    }
}
