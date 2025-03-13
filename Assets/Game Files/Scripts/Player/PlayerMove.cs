using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�ړ����x"), SerializeField]
    private float speed = 0;

    // ���݂̈ړ����x
    private float moveX = 0;
    private float moveZ = 0;

    private float playerAngle = 0;   // �v���C���[�̌���

    [Header("�㑤�̓����蔻��"), SerializeField] private WallHitCollision colliderFront;
    [Header("�����̓����蔻��"), SerializeField] private WallHitCollision colliderBack;
    [Header("�����̓����蔻��"), SerializeField] private WallHitCollision colliderLeft;
    [Header("�E���̓����蔻��"), SerializeField] private WallHitCollision colliderRight;

    public enum MoveState   // �ړ����
    {
        front,   // �O�i
        back,    // ���
        left,    // ��
        right,   // �E
        stop,    // ��~
        none     // �Ȃ�
    }
    public MoveState moveState;       // ���݈ړ����Ă������
    public MoveState moveStateSave;   // �ǂ̕����ֈړ����邩��ۑ�����

    private Rigidbody rb;

    private PlayerBase player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = GetComponent<PlayerBase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveState = MoveState.stop;
        moveStateSave = MoveState.none;
        UpdateParam();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsDead)
        {
            moveStateSave = MoveState.none;
            moveState = MoveState.stop;
            UpdateParam();

            return;
        }

        InputMove();
        ChangeMoveState();

        // ���x�ƕ����̓K�p
        rb.velocity = new Vector3(moveX, 0, moveZ);
    }

    private void InputMove()   // ����
    {
        // ���͎󂯎��
        if (Input.GetKey(KeyCode.W))
        {
            moveStateSave = MoveState.front;   // �ړ�������ۑ�
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveStateSave = MoveState.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveStateSave = MoveState.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveStateSave = MoveState.right;
        }
    }

    private bool IsWall(WallHitCollision anyCollider)   // �󂯎�����R���C�_�[�ɕǂ��G��Ă��邩�`�F�b�N
    {
        return anyCollider.IsHit;
    }

    private void ChangeMoveState()
    {
        // �ۑ������ړ������ɕǂ��Ȃ��ꍇ
        if (moveStateSave == MoveState.front && !IsWall(colliderFront))
        {
            moveState = MoveState.front;   // �ړ�������ύX
            UpdateParam();                 // �e�p�����[�^�[���X�V
        }
        if (moveStateSave == MoveState.back && !IsWall(colliderBack))
        {
            moveState = MoveState.back;
            UpdateParam();
        }
        if (moveStateSave == MoveState.left && !IsWall(colliderLeft))
        {
            moveState = MoveState.left;
            UpdateParam();
        }
        if (moveStateSave == MoveState.right && !IsWall(colliderRight))
        {
            moveState = MoveState.right;
            UpdateParam();
        }
    }

    private void UpdateParam()   // �ړ��Ɋւ���l�̍X�V
    {
        var oldAngle = playerAngle;   // �X�V�O�̌�����ۑ�

        switch (moveState)
        {
            // �ړ���Ԃɉ����đ��x�ƌ�����ύX
            case MoveState.front:   // �O�i���̏ꍇ
                moveX = 0;
                moveZ = speed;
                playerAngle = 0;
                break;

            case MoveState.back:    // ��ޒ��̏ꍇ
                moveX = 0;
                moveZ = -speed;
                playerAngle = 180;
                break;

            case MoveState.left:    // ���ړ����̏ꍇ
                moveX = -speed;
                moveZ = 0;
                playerAngle = 270;
                break;

            case MoveState.right:   // �E�ړ����̏ꍇ
                moveX = speed;
                moveZ = 0;
                playerAngle = 90;
                break;

            case MoveState.stop:    // ��~���̏ꍇ
                moveX = 0;
                moveZ = 0;
                break;
        }

        if (player.IsDead) return;
        if (oldAngle == playerAngle) return;   // �������ς���Ă��Ȃ������ꍇ�I��

        transform.DOLocalRotate(new Vector3(0, playerAngle, 0), 0.1f);   // �v���C���[�̉�]
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ����̃^�C�~���O�Œ�~��Ԃ�
        if (IsWall(colliderFront) && moveState == MoveState.front) moveState = MoveState.stop;
        if (IsWall(colliderBack)  && moveState == MoveState.back)  moveState = MoveState.stop;
        if (IsWall(colliderLeft)  && moveState == MoveState.left)  moveState = MoveState.stop;
        if (IsWall(colliderRight) && moveState == MoveState.right) moveState = MoveState.stop;
    }

    public float Speed
    {
        get { return speed; }
    }

    private void OnDestroy()
    {
        DOTween.Clear(true);
    }
}
