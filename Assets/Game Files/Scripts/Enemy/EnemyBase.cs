using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public enum MovePattern   // �s���p�^�[��
    {
        roaming,   // �꒣��p�j
        chase,     // �v���C���[�ǐ�
        //izike      // ������
    }
    public MovePattern movePattern;

    private bool isIzike = false;   // ��������ԃt���O

    private float speedNormal = 0f;   // �ʏ푬�x
    private float speedIzike = 0;       // ��������Ԃ̑��x
    private float speedNow = 0;         // ���݂̑��x

    private Vector3 targetPos = Vector3.zero;

    [Header("�s���p�^�[�����؂�ւ��̂ɂ����鎞��"), SerializeField]
    private float patternChangeTime = 0;
    private float patternChangeTimer = 0;   // �؂�ւ����ԃJ�E���g�p�ϐ�
    private float izikeTimer = 0;           // ��������Ԃ̃^�C�}�[�p�ϐ�

    [Header("�����̖�"), SerializeField]
    private GameObject[] eyes;

    private bool isDead = false;

    [Header("�X�|�[�����Ă��瓮�������܂ł̑ҋ@����"), SerializeField]
    private float moveDelay = 0;
    private bool isStartMove = false;

    [Header("���X�|�[���ɂ����鎞��"), SerializeField]
    private float respawnTime;
    [Header("���X�|�[������n�_"), SerializeField]
    private Transform respawnPoint;
    private Vector3 startPosition;

    [Header("�����̓꒣��"), SerializeField]
    private TerritoryChecker territory;
    private Transform[] movePoints;

    private Color defaultColor;   // ���̐F�ۑ��ϐ�

    [Header("�v���C���["), SerializeField]
    protected PlayerBase player;

    [Header("���b�V���̏ꏊ"), SerializeField]
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
        startPosition = this.transform.position;   // �Q�[���J�n���̈ʒu��ۑ�

        GetMovePoints();

        movePattern = MovePattern.roaming;   // �ŏ��̍s���p�^�[����ݒ�

        speedNormal = FindAnyObjectByType<PlayerMove>().Speed * 0.8f;   // �ʏ�ړ����x���v���C���[��菭���x���ݒ�
        speedIzike = speedNormal / 2;   // ��������Ԃ̈ړ����x�̌v�Z
        speedNow = speedNormal;         // ���x��K�p

        defaultColor = _renderer.material.color;   // �f�t�H���g�̐F�̕ۑ�

        StartCoroutine(StartMove());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();   // ���x�̕ύX�ƓK�p

        if (isDead) return;

        ChangeTimer();
        IzikeChanger();
        SetMovePos();
    }

    private void GetMovePoints()   // �꒣��̎q�I�u�W�F�N�g���擾����
    {
        var children = territory.gameObject.transform.childCount;
        movePoints = new Transform[children];

        for(int i = 0; i < children; i++)
        {
            movePoints[i] = territory.transform.GetChild(i);
        }
    }

    private void SetMovePos()   // �ړ��n�_�̐ݒ�
    {
        if (agent.enabled == true) agent.destination = targetPos;
    }

    private IEnumerator StartMove()   // �X�|�[�����瑃���o��܂őҋ@���Ԃ�݂���
    {
        isStartMove = false;
        targetPos = transform.position;

        yield return new WaitForSeconds(moveDelay);

        isStartMove = true;

        UpdateMovePoint(true);
    }

    private void UpdateMovePoint(bool isStart)   // �꒣����ɂ�����ړ��n�_�̕ύX
    {
        // �ړI�n�ƃG�[�W�F���g�̋�����0.5�ȓ��ł��菉���łȂ��ꍇ��̏�����
        if (agent.remainingDistance > 0.5f && !isStart) return;

        // �����_���ňړ�����ꏊ������
        int pointNum = Random.Range(0, movePoints.Length);
        var nextPoint = movePoints[pointNum].position;

        targetPos = nextPoint;
    }

    private void ChangePattern()   // �s���p�^�[����ύX
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
        // ��������ԂłȂ��ꍇ
        if (isIzike) return;

        if (patternChangeTimer <= patternChangeTime)   // �w�肵�����Ԍo�߂���܂�
        {
            patternChangeTimer += Time.deltaTime;
        }
        else   // �w�肵�����Ԍo�߂�����
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

        if (isIzike)   // ��������Ԃ̏ꍇ
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

        if (movePattern == MovePattern.roaming)   // �p�j���̏ꍇ
        {
            UpdateMovePoint(false);

            if (territory.IsInArea) targetPos = player.transform.position;
        }
        else   // �ǐՒ��̏ꍇ
        {
            targetPos = player.transform.position;
        }
    }

    public void StartIzike()
    {
        izikeTimer = player.PowerUpTime;   // �^�C�}�[��ݒ�

        // �����_���ňړ�����ꏊ������
        int pointNum = Random.Range(0, movePoints.Length);
        var nextPoint = movePoints[pointNum].position;

        targetPos = nextPoint;
    }

    private void IzikeChanger()   // ��������Ԃ̐؂�ւ�
    {
        if(izikeTimer > 0)
        {
            isIzike = true;
            izikeTimer -= Time.deltaTime;   // �^�C�������炷
        }
        else
        {
            isIzike = false;
        }
    }

    private IEnumerator Dead()
    {
        isDead = true;

        izikeTimer = 0;   // ��������Ԃ�����

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

        movePattern = MovePattern.roaming;   // �p�j��Ԃ֖߂�

        isDead = false;

        targetPos = transform.position;

        StartCoroutine(StartMove());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;   // �G�ꂽ���̂��v���C���[�łȂ��ꍇ�I��

        if (isIzike) StartCoroutine(Dead());   // �����̎��S����
        else         player.Dead();            // �v���C���[�̎��S����
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    //public void UpdateTarget()   // �v���C���[�Č����i�v���C���[�������������p�j
    //{
    //    player = FindAnyObjectByType<PlayerBase>();
    //}

    //public void StartDead()
    //{
    //    StartCoroutine(Dead());
    //}
}
