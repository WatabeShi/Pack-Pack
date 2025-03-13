using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeySpawner : MonoBehaviour
{
    [Header("��������S�[���̌�"), SerializeField]
    private GameObject goalKey;

    [Header("��������ꏊ���̂܂Ƃ܂�"), SerializeField]
    private GameObject spawnGroup;
    private Transform[] spawnPoints;

    private bool spawnTrigger = true;   // �����p�g���K�[

    private void Awake()
    {
        GetSpawnPoint();   // ���̐����n�_�̎擾
    }

    public void SpawnGoalKey()   // ���̐�������
    {
        if (!spawnTrigger) return;

        int pointNum = Random.Range(0, spawnPoints.Length);
        var key = Instantiate(goalKey, spawnPoints[pointNum].position, goalKey.transform.rotation);
        key.transform.SetParent(transform);
        spawnTrigger = false;   // �g���K�[������
    }

    private void GetSpawnPoint()
    {
        spawnPoints = new Transform[spawnGroup.transform.childCount];   // �z��̒�`

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = spawnGroup.transform.GetChild(i).transform;   // �q�I�u�W�F�N�g��z���
        }
    }
}
