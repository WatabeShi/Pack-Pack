using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Cinemachine;

public class EnemyBaseFPS : MonoBehaviour
{
    [Header("�ő�HP"), SerializeField]
    private int hp = 50;
    private int hpMax;   // �ő�HP�ۑ��p�ϐ�

    [Header("�ړ����x"), SerializeField]
    private float speedMove = 5;

    [Header("�ړ��͈́iX���j"), SerializeField]
    private float rangeX;
    [Header("�ړ��͈́iZ���j"), SerializeField]
    private float rangeZ;
    Vector3 movePos = Vector3.zero;

    [Header("�v���C���["), SerializeField]
    private Transform player;

    [Header("�v���C���[��ǐՂ���������"), SerializeField]
    private float chaseDistance = 0;

    [Header("�����ɂ����鎞��"), SerializeField]
    private float respawnTime = 0;

    [Header("���S���̃G�t�F�N�g"), SerializeField]
    private GameObject deadEffect;
    [Header("���SSE"), SerializeField]
    private AudioClip deadSE;

    [Header("�����n�_�̂܂Ƃ܂�"), SerializeField]
    private GameObject rsPointGroup;
    private TerritoryChecker[] respawnPoints;    // �S�Ẵ��X�|�[���n�_
    private TerritoryChecker[] _respawnPoints;   // �v���C���[���߂��ɂ��Ȃ��ꏊ
    private int pointCount = 0;                  // ���X�|�[���ł���n�_�̐�

    [Header("�|���ꂽ���ɉ��Z�����X�R�A"), SerializeField]
    private int killScore;

    [Header("���ږ�"), SerializeField]
    private GameObject[] eyes;

    [Header("�~�j�}�b�v�p�A�C�R��"), SerializeField]
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

        hpMax = hp;   // �ő�HP�̕ۑ�
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = IsDead() ? 0 : speedMove;   // ��Ԃɉ����đ��x��ύX

        ChangeDestination();
        LookPlayer();
    }

    private void GetStartRsPoint()   // ���X�|�[���n�_�̎擾�i�Q�[���J�n���p�j
    {
        pointCount = rsPointGroup.transform.childCount;
        respawnPoints = new TerritoryChecker[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            respawnPoints[i] = rsPointGroup.transform.GetChild(i).gameObject.GetComponent<TerritoryChecker>();
        }
    }

    private void GetRsPoint()   // ���X�|�[���n�_�̎擾�i���S���p�j
    {
        _respawnPoints = null;   // ������
        _respawnPoints = respawnPoints.Where(e => !e.IsInArea).ToArray();   // ���͈͓̔��Ƀv���C���[�����Ȃ��ꏊ��z��ɒǉ�

        pointCount = _respawnPoints.Length;   // ���X�|�[���ł���n�_�̐��̍X�V
    }

    private void ChangeMovePos()   // �ړ��n�_�̐ݒ�
    {
        // �w�肵���͈͓��Ń����_���Ō���
        var moveX = Random.Range(-rangeX, rangeX);
        var moveZ = Random.Range(-rangeZ, rangeZ);
        movePos = new Vector3(moveX, transform.position.y, moveZ);
    }

    private void ChangeDestination()   // �ړ��n�_�̍X�V
    {
        if (IsDead()) return;

        if (agent.remainingDistance < 1) ChangeMovePos();   // �ڕW�n�_�Ƃ̋������P�ȓ���������

        agent.destination = IsNearPlayer() ? player.position : movePos;   // �v���C���[���߂��ɂ��邩�ňړ��n�_��ύX
    }

    private bool IsNearPlayer()   // �v���C���[�����ӂɂ��邩����
    {
        var playerVec = player.position - this.transform.position;   // �v���C���[�Ǝ����̃x�N�g���v�Z

        if (playerVec.magnitude <= chaseDistance) return true;   // �w�肵���������Ƀv���C���[������ꍇ true
        else return false;
    }

    public void Damage(int damage)   // �_���[�W���󂯂鏈��
    {
        if (IsDead()) return;   // ����ł������̏��������s���Ȃ�

        hp -= damage;            // HP�����炷
        hp = Mathf.Max(hp, 0);   // �l�̒���

        if (IsDead()) Dead();   // ���S�֐�
    }

    private bool IsDead()   // ���S����
    {
        return (hp <= 0) ? true : false;
    }

    private void Dead()
    {
        if (scoreManager != null) scoreManager.AddScore(killScore);   // �X�R�A�̉��Z

        minimapIcon.SetActive(false);   // �~�j�}�b�v�A�C�R��������
        foreach (GameObject eye in eyes) eye.SetActive(false);   // �ڂ̔�\��

        // �����_���[�ƃR���C�_�[������
        foreach (Renderer ren in _renderers) ren.enabled = false;
        myCollider.enabled = false;

        var _deadEffect = Instantiate(deadEffect, this.transform.position, Quaternion.identity);   // �G�t�F�N�g����
        _deadEffect.transform.localScale = new Vector3(2, 2, 2);   // �G�t�F�N�g�̃T�C�Y����    
        audioSource.PlayOneShot(deadSE);   // SE�Đ�

        var dir = this.transform.position - player.position;

        impSource.GenerateImpulse();
        //Vector3 shakePow = new Vector3(5, 5, 5);
        //Camera.main.transform.DOShakeRotation(0.5f, shakePow).SetRelative();   // �J�����̗h��

        StartCoroutine(Respawn());   // �����R���[�`���N��
    }

    private IEnumerator Respawn()   // ����
    {
        yield return new WaitForSeconds(respawnTime);   // �w�肵�����ԕ��ҋ@

        // �����_���[�ƃR���C�_�[�L����
        myCollider.enabled = true;
        foreach (Renderer ren in _renderers) ren.enabled = true;

        foreach (GameObject eye in eyes) eye.SetActive(true);   // �ڂ̕\��
        minimapIcon.SetActive(true);   // �~�j�}�b�v�A�C�R����\��

        // �]������
        GetRsPoint();   // �]���ł���ꏊ�̎擾
        int rsPointNum = Random.Range(0, pointCount);   // �擾�����ꏊ�̒����烉���_���ŏꏊ������
        this.transform.position = _respawnPoints[rsPointNum].gameObject.transform.position;   // ���肵���ꏊ�ɓ]��

        hp = hpMax;   // HP���S��
    }

    private void LookPlayer()   // �ڂ���Ƀv���C���[���������鏈��
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
        StopAllCoroutines();   // �S�ẴR���[�`�����~
    }
}
