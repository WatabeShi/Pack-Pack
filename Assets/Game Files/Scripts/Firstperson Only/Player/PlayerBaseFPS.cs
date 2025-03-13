using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerBaseFPS : PlayerBase
{
    [Header("�����n�_"), SerializeField]
    private GameObject rsPointGroup;
    //private Transform[] respawnPoints;

    [Header("�����ɂ����鎞��"), SerializeField]
    private float respawnTime = 0;
    private EnemyChecker[] respawnPoints;    // �S�Ẵ��X�|�[���n�_
    private EnemyChecker[] _respawnPoints;   // �v���C���[���߂��ɂ��Ȃ��ꏊ
    private int pointCount = 0;                  // ���X�|�[���ł���n�_�̐�

    [Header("���S����SE"), SerializeField]
    private AudioClip deadSE;

    private Color color = Color.black;
    private float fadeAlpha = 0;
    [Header("�t�F�[�h���x"), SerializeField]
    private float fadeSpeed = 5;

    private bool hasKey = false;   // �S�[���̌��������Ă��邩

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

        Spawn();   // �����_���ȏꏊ�ɃX�|�[���i�ꏊ�ړ��j
    }

    // Update is called once per frame
    void Update()
    {
        PowerupProcess();
        PushButton();
    }

    private void Spawn()   // �����_���ȏꏊ�ɃX�|�[���i�ꏊ�ړ��j
    {
        GetStartPoint();

        int spawnPointNum = SelectSpawnNum();
        transform.position = respawnPoints[spawnPointNum].transform.position;
    }

    private void GetStartPoint()   // �J�n�n�_�̎擾
    {
        pointCount = rsPointGroup.transform.childCount;
        respawnPoints = new EnemyChecker[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            respawnPoints[i] = rsPointGroup.transform.GetChild(i).GetComponent<EnemyChecker>();
        }
    }

    private void GetRespawnPoint()   // ���X�|�[���n�_�̎擾�i���S���p�j
    {
        _respawnPoints = null;   // ������
        _respawnPoints = respawnPoints.Where(e => !e.IsInArea).ToArray();   // ���͈͓̔��ɓG�����Ȃ��ꏊ��z��ɒǉ�

        pointCount = _respawnPoints.Length;   // ���X�|�[���ł���n�_���̍X�V
    }

    private IEnumerator DeadRespawn(Collider _other)   // ���S���畜���܂ł̈�A�̗���
    {
        if (!_other.gameObject.CompareTag("Enemy")) yield break;

        Dead();

        yield return new WaitForSeconds(respawnTime);

        Respawn();
    }

    public override void Dead()   // ���S����
    {
        isDead = true;

        StartCoroutine(Fade());   // �t�F�[�h���o

        lifeManager.Damage();   // �c�@�����炷

        weaponManager.DisableWeapons();   // ��������

        myCollider.enabled = false;   // �����蔻�薳����
        rb.isKinematic = true;        // Rigidbody������

        SESource.instance.PlaySE(deadSE);   // SE�Đ�
    }

    public override void Respawn()   // ����
    {
        if (lifeManager.IsGameOver())
        {
            FindAnyObjectByType<GameManagerFPS>().GameOver();   // �Q�[���I�[�o�[�����̌Ăяo��
        }
        else
        {
            isDead = false;   // �t���O�L����

            StartCoroutine(Fade());

            myCollider.enabled = true;
            rb.isKinematic = false;

            GetRespawnPoint();   // �����\�n�_�̎擾
            int rspNum = SelectSpawnNum();
            transform.position = _respawnPoints[rspNum].transform.position;
        }
    }

    private int SelectSpawnNum()   // �ړ�����ꏊ�������_���Ō���
    {
        return Random.Range(0, pointCount);
    }

    private void PushButton()   // �{�^��������
    {
        if (isDead) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;

        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Ray ray = Camera.main.ViewportPointToRay(screenCenter);   // ���C����ʒ��������΂�
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            float displayTime = 3f;   // �f�o�b�O���C�L���X�g�̕\������
            Debug.DrawRay(ray.origin, ray.direction * 2.5f, Color.yellow, displayTime);

            if (hit.transform.CompareTag("Switch")) hit.collider.GetComponent<GoalSwitch>().OpenGoal(hasKey);
        }
    }

    private IEnumerator Fade()   // ���S���̃t�F�[�h���o
    {
        if (isDead)
        {
            while (fadeAlpha < 1)
            {
                fadeAlpha += fadeSpeed * Time.deltaTime;   // �w�i�̃A���t�@�l�̑���

                yield return null;
            }
        }
        else
        {
            while (fadeAlpha > 0)
            {
                fadeAlpha -= fadeSpeed * Time.deltaTime;   // �w�i�̃A���t�@�l�̌���

                yield return null;
            }
        }
    }

    private void GetKey(Collider other)   // ���̓��菈��
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
