using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItemSpowner : MonoBehaviour
{
    [Header("�����ɕK�v�ȃG�T�̐�"), SerializeField]
    private int[] needBaitCount;

    [Header("��������A�C�e��"), SerializeField]
    private GameObject[] bonusItems;

    private bool spawnTrigger = true;     // ���ڗp�����g���K�[
    private bool isSecondSpown = false;   // ���ڂ̐�����

    private GameManagerTPS gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManagerTPS>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSecondSpown)   // �ŏ��̐����̏ꍇ
        {
            if (gameManager.EatBaitCount >= needBaitCount[0])   // �H�ׂ��G�T�����ݒ萔���z�����ꍇ
            {
                Instantiate(ItemSelector(), transform.position, Quaternion.identity);
                isSecondSpown = true;
            }
        }
        else   // ���ڂ̐����̏ꍇ
        {
            if (gameManager.EatBaitCount >= needBaitCount[1])   // �H�ׂ��G�T�����ݒ萔���z�����ꍇ
            {
                if (spawnTrigger)
                {
                    Instantiate(ItemSelector(), transform.position, Quaternion.identity);
                    spawnTrigger = false;
                }
            }
        }
    }

    private GameObject ItemSelector()   // ���݂̃X�e�[�W�ɉ����Đ�������A�C�e����ύX
    {
        int num = 0;

        if (GameManagerTPS.stageNum == 1) num = 0;                                  // ��������
        if (GameManagerTPS.stageNum == 2) num = 1;                                  // �
        if (GameManagerTPS.stageNum == 3 || GameManagerTPS.stageNum == 4) num = 2;     // �I�����W
        if (GameManagerTPS.stageNum == 5 || GameManagerTPS.stageNum == 6) num = 3;     // ���
        if (GameManagerTPS.stageNum == 7 || GameManagerTPS.stageNum == 8) num = 4;     // ������
        if (GameManagerTPS.stageNum == 9 || GameManagerTPS.stageNum == 10) num = 5;    // �{�X�E�M�����N�V�A��
        if (GameManagerTPS.stageNum == 11 || GameManagerTPS.stageNum == 12) num = 6;   // �x��
        if (GameManagerTPS.stageNum >= 13) num = 7;                                 // ��

        return bonusItems[num];
    }
}
