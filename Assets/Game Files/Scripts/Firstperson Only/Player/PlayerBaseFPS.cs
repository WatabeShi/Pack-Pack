using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerBaseFPS : PlayerBase
{
    [Header("復活地点"), SerializeField]
    private GameObject rsPointGroup;
    //private Transform[] respawnPoints;

    [Header("復活にかかる時間"), SerializeField]
    private float respawnTime = 0;
    private EnemyChecker[] respawnPoints;    // 全てのリスポーン地点
    private EnemyChecker[] _respawnPoints;   // プレイヤーが近くにいない場所
    private int pointCount = 0;                  // リスポーンできる地点の数

    [Header("死亡時のSE"), SerializeField]
    private AudioClip deadSE;

    private Color color = Color.black;
    private float fadeAlpha = 0;
    [Header("フェード速度"), SerializeField]
    private float fadeSpeed = 5;

    private bool hasKey = false;   // ゴールの鍵を持っているか

    private LifeManager lifeManager;
    private WeaponManager weaponManager;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        lifeManager = FindAnyObjectByType<LifeManager>();
        weaponManager = FindAnyObjectByType<WeaponManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeAlpha = 0;

        Spawn();   // ランダムな場所にスポーン（場所移動）
    }

    // Update is called once per frame
    void Update()
    {
        PowerupProcess();
        PushButton();
    }

    private void Spawn()   // ランダムな場所にスポーン（場所移動）
    {
        GetStartPoint();

        int spawnPointNum = SelectSpawnNum();
        transform.position = respawnPoints[spawnPointNum].transform.position;
    }

    private void GetStartPoint()   // 開始地点の取得
    {
        pointCount = rsPointGroup.transform.childCount;
        respawnPoints = new EnemyChecker[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            respawnPoints[i] = rsPointGroup.transform.GetChild(i).GetComponent<EnemyChecker>();
        }
    }

    private void GetRespawnPoint()   // リスポーン地点の取得（死亡時用）
    {
        _respawnPoints = null;   // 初期化
        _respawnPoints = respawnPoints.Where(e => !e.IsInArea).ToArray();   // 一定の範囲内に敵がいない場所を配列に追加

        pointCount = _respawnPoints.Length;   // リスポーンできる地点数の更新
    }

    private IEnumerator DeadRespawn(Collider _other)   // 死亡から復活までの一連の流れ
    {
        if (!_other.gameObject.CompareTag("Enemy")) yield break;

        Dead();

        yield return new WaitForSeconds(respawnTime);

        Respawn();
    }

    public override void Dead()   // 死亡処理
    {
        isDead = true;

        StartCoroutine(Fade());   // フェード演出

        lifeManager.Damage();   // 残機を減らす

        weaponManager.DisableWeapons();   // 武装解除

        myCollider.enabled = false;   // 当たり判定無効化
        rb.isKinematic = true;        // Rigidbody無効化

        SESource.instance.PlaySE(deadSE);   // SE再生
    }

    public override void Respawn()   // 復活
    {
        if (lifeManager.IsGameOver())
        {
            FindAnyObjectByType<GameManagerFPS>().GameOver();   // ゲームオーバー処理の呼び出し
        }
        else
        {
            isDead = false;   // フラグ有効化

            StartCoroutine(Fade());

            myCollider.enabled = true;
            rb.isKinematic = false;

            GetRespawnPoint();   // 復活可能地点の取得
            int rspNum = SelectSpawnNum();
            transform.position = _respawnPoints[rspNum].transform.position;
        }
    }

    private int SelectSpawnNum()   // 移動する場所をランダムで決定
    {
        return Random.Range(0, pointCount);
    }

    private void PushButton()   // ボタンを押す
    {
        if (isDead) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;

        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Ray ray = Camera.main.ViewportPointToRay(screenCenter);   // レイを画面中央から飛ばす
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            float displayTime = 3f;   // デバッグレイキャストの表示時間
            Debug.DrawRay(ray.origin, ray.direction * 2.5f, Color.yellow, displayTime);

            if (hit.transform.CompareTag("Switch")) hit.collider.GetComponent<GoalSwitch>().OpenGoal(hasKey);
        }
    }

    private IEnumerator Fade()   // 死亡時のフェード演出
    {
        if (isDead)
        {
            while (fadeAlpha < 1)
            {
                fadeAlpha += fadeSpeed * Time.deltaTime;   // 背景のアルファ値の増加

                yield return null;
            }
        }
        else
        {
            while (fadeAlpha > 0)
            {
                fadeAlpha -= fadeSpeed * Time.deltaTime;   // 背景のアルファ値の減少

                yield return null;
            }
        }
    }

    private void GetKey(Collider other)   // 鍵の入手処理
    {
        if (!other.gameObject.CompareTag("Goal Key")) return;

        hasKey = true;
        Destroy(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        StartCoroutine(DeadRespawn(other));
        GetKey(other);
    }

    public bool HasKey
    {
        get { return hasKey; }
    }

    private void OnGUI()
    {
        if(!isDead) return;

        color.a = fadeAlpha;
        GUI.color = color;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
    }
}
