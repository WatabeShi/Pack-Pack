using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected bool isPowerup = false;   // �p���[�A�b�v��ԃt���O
    private float powerupTimeCounter = 0;
    [Header("�p���[�A�b�v����"), SerializeField]
    private int powerupTime = 0;

    protected bool isDead = false;

    [Header("���X�|�[������n�_"), SerializeField]
    private Transform respawnPoint;

    [Header("���b�V���̂���I�u�W�F�N�g"), SerializeField]
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
        if(powerupTimeCounter <= 0)   // �^�C�}�[���O�ȉ��ɂȂ�����
        {
            isPowerup = false;
        }
        else
        {
            isPowerup = true;

            powerupTimeCounter -= 1 * Time.deltaTime;   // �^�C�}�[�J�E���g�_�E��
        }
    }

    public virtual void Dead()   // ���񂾍ۂ̏���
    {
        //Destroy(this.gameObject);
        //StartCoroutine(Respawn());

        isDead = true;

        _renderer.enabled = false;
        myCollider.enabled = false;
        rb.isKinematic = true;

        gameManager.StartPlayerDeadProcess();
    }

    public virtual void Respawn()   // ����
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

        //if (!isPowerup)   // �p���[�A�b�v���ł͂Ȃ��ꍇ
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
