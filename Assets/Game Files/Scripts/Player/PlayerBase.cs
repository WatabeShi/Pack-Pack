using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected bool isPowerup = false;   // パワーアップ状態フラグ
    private float powerupTimeCounter = 0;
    [Header("パワーアップ時間"), SerializeField]
    private int powerupTime = 0;

    protected bool isDead = false;

    [Header("リスポーンする地点"), SerializeField]
    private Transform respawnPoint;

    [Header("メッシュのあるオブジェクト"), SerializeField]
    protected Renderer _renderer;
    protected Collider myCollider;
    protected Rigidbody rb;

    private GameManagerTPS gameManager;
    private PlayerMove mover;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = _renderer.GetComponent<Renderer>();
        myCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        gameManager = FindAnyObjectByType<GameManagerTPS>();
        mover = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        PowerupProcess();
    }

    public void StartPowerUp()
    {
        powerupTimeCounter = powerupTime;
    }

    protected void PowerupProcess()
    {
        if(powerupTimeCounter <= 0)   // タイマーが０以下になったら
        {
            isPowerup = false;
        }
        else
        {
            isPowerup = true;

            powerupTimeCounter -= 1 * Time.deltaTime;   // タイマーカウントダウン
        }
    }

    public virtual void Dead()   // 死んだ際の処理
    {
        //Destroy(this.gameObject);
        //StartCoroutine(Respawn());

        isDead = true;

        _renderer.enabled = false;
        myCollider.enabled = false;
        rb.isKinematic = true;

        gameManager.StartPlayerDeadProcess();
    }

    public virtual void Respawn()   // 復活
    {
        isDead = false;

        this.transform.position = respawnPoint.position;
        _renderer.enabled = true;
        myCollider.enabled = true;
        rb.isKinematic = false;
    }

    //private IEnumerator Respawn()
    //{
    //    isDead = true;

    //    mRenderer.enabled = false;
    //    sCollider.enabled = false;
    //    rb.isKinematic = true;

    //    yield return new WaitForSeconds(3);

    //    this.transform.position = respawnPoint.position;
    //    mRenderer.enabled = true;
    //    sCollider.enabled = true;
    //    rb.isKinematic = false;

    //    //mover.ResetParam();
    //    isDead = false;
    //}

    private void OnTriggerEnter(Collider other)
    {
        //if (!other.gameObject.CompareTag("Enemy")) return;

        //if (!isPowerup)   // パワーアップ中ではない場合
        //{
        //    Dead();
        //}
    }

    public bool IsDead
    {
        get { return isDead; }
    }
    public int PowerUpTime
    {
        get { return powerupTime; }
    }
    public bool IsPowerup
    {
        get { return isPowerup; }
    }
}
