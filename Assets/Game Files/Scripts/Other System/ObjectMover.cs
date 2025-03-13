using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    // �v���C���[�̃I�u�W�F�N�g
    [Header("�^�[�Q�b�g�ƂȂ�I�u�W�F�N�g"), SerializeField]
    private GameObject targetObj;

    // �J�����̃v���C���[�̋���
    private Vector3 offset;

    private GameManager gameManager;

    private PlayerBase player;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        player = FindAnyObjectByType<PlayerBase>();

        offset = transform.position - targetObj.transform.position;
    }

    void Update()
    {
        if (player.IsDead) return;

        transform.position = targetObj.transform.position + offset;
    }

    public void UpdateTarget()   // �^�[�Q�b�g�̃A�b�v�f�[�g
    {
        player = FindAnyObjectByType<PlayerBase>();
        targetObj = player.gameObject;

        transform.position = targetObj.transform.position + offset;
    }
}
