using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTPS : GameManager
{
    private bool isClear = false;   // �X�e�[�W�N���A�t���O

    //private bool playerDead = false;

    private int stageNowNum = 1;      // �J�n�X�e�[�W���C���X�y�N�^�[��Őݒ肷��p�ϐ�
    public static int stageNum = 1;   // ���݂̃X�e�[�W

    private ObjectMover[] objMover;
    private PlayerBase player;

    new void Awake()
    {
        base.Awake();

        objMover = FindObjectsByType<ObjectMover>(FindObjectsSortMode.None);
        player = FindAnyObjectByType<PlayerBase>();
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        isClear = false;
        stageNum = stageNowNum;
    }

    // Update is called once per frame
    void Update()
    {
        DispLife();   // ���C�t�̕\��

        // �S�ẴG�T��H�ׂ���N���A
        if (eatBaitCount == baitCount) isClear = true;
    }

    private void StartClearProcess()   // �N���A���Ɉ�x�Ă΂�鏈��
    {
        int delayTime = 1;
        StartCoroutine(ClearProcess(delayTime));
    }

    private IEnumerator ClearProcess(float delay)   // �N���A���̏���
    {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1;

        if(IsCoffeeBreakStage())   // �R�[�q�[�u���C�N�̕K�v�ȃX�e�[�W��
        {
            int breakTime = 1;   // �A�j���[�V�����Đ�����
            yield return StartCoroutine(CoffeeBreak(breakTime));   // �R�[�q�[�u���C�N�A�j���[�V�������Đ����Đ��I���܂Ŏ��̏�����ҋ@
        }

        stageNum++;   // �X�e�[�W���̉��Z
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   // �X�e�[�W�č\�z
    }

    private bool IsCoffeeBreakStage()   // �R�[�q�[�u���C�N�̕K�v�ȃX�e�[�W��
    {
        if(stageNum == 2 || stageNum == 5 || stageNum == 9 || stageNum == 13 || stageNum == 17)   // ����̃X�e�[�W�̏ꍇ
        {
            return true;   // �R�[�q�[�u���C�N��L����
        }
        else
        {
            return false;   // �R�[�q�[�u���C�N�𖳌���
        }
    }

    private IEnumerator CoffeeBreak(float delay)   // �R�[�q�[�u���C�N�̏���
    {
        if(stageNum == 2)
        {
            print("�X�e�[�W�Q�R�[�q�[�u���C�N������");
        }
        else if(stageNum == 5)
        {
            print("�X�e�[�W�T�R�[�q�[�u���C�N������");
        }
        else
        {
            print("�X�e�[�W�H�R�[�q�[�u���C�N������");
        }

        yield return new WaitForSeconds(delay);
    }

    public void StartPlayerDeadProcess()   // �v���C���[���S���̏������J�n������
    {
        // ���S�t���O���L���̏ꍇ���s���Ȃ�
        //if (player.IsDead) return;

        int delayTime = 3;   // ���S�A�j���[�V�����Đ�����
        StartCoroutine(PlayerDeadProcess(delayTime));
    }

    private IEnumerator PlayerDeadProcess(float delay)   // �v���C���[���S���̏���
    {
        //playerDead = true;   // ���S�t���O�L����
        //lifeCount--;         // �c�@�����炷
        lifeManager.Damage();
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1;

        if(!lifeManager.IsGameOver())
        {
            // �J�����A�G�A�G�T�̃^�[�Q�b�g�Đݒ�
            foreach(ObjectMover _objMover in objMover)
            {
                _objMover.UpdateTarget();
            }
            foreach(EnemyBase enemy in enemies)
            {
                enemy.Respawn(false);
            }
            foreach(PowerUpBait puBait in puBaits)
            {
                puBait.UpdateTarget();
            }

            player.Respawn();   // �v���C���[�̃��X�|�[��

            //playerDead = false;   // ���S�t���O������
        }
        else   // �c�@���Ȃ���
        {
            GameOver();   // �Q�[���I�[�o�[����
        }
    }

    private void OnEnable()
    {
        EventManager.clearEvent += StartClearProcess;         // �N���A�������C�x���g�ɑ��
        //EventManager.gameoverEvent += GameoverProcess;   // �Q�[���I�[�o�[�������C�x���g�ɑ��
    }

    private void OnDisable()
    {
        EventManager.clearEvent -= StartClearProcess;      // �N���A�������C�x���g���珜�O
        //EventManager.GameoverEvent -= GameoverProcess;   // �Q�[���I�[�o�[�������C�x���g���珜�O
    }

    //public bool PlayerDead
    //{
    //    get { return playerDead; }
    //}

    public new void GameOver()   // �Q�[���I�[�o�[����
    {
        base.GameOver();

        ScoreRanking.instance.RankInTPS(ScoreManager.scoreNow);

        //BGMSource.instance.StopBGM();
    }

    public bool IsClear
    {
        get { return isClear; }
    }
}
