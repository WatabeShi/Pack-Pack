using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveFPS : MonoBehaviour
{
    [Header("���s���x"), SerializeField]
    private float speedWalk = 0;
    [Header("�_�b�V�����x"), SerializeField]
    private float speedDash = 0;
    private float speedNow = 0;   // ���݂̑��x

    [Header("�J�������x"), SerializeField]
    private float lookSensitivity = 5f;
    [Header("�v���C���[�̎��_�ƂȂ�J����"), SerializeField]
    GameObject fpsCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    private float cameraUpAndDownRotation = 0f;         //���_�㉺�p�̕ϐ�
    private float currentCameraUpAndDownRotation = 0f;  //���_�㉺�p�̕ϐ�

    private Rigidbody rb;

    private PlayerBaseFPS player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerBaseFPS>();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
        if (player.IsDead) return;

        CameraControl();
        Move();
    }

    void CameraControl()   // �J��������
    {
        // ���E���_
        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _rotationVector = new Vector3(0, _yRotation, 0) * lookSensitivity;
        rotation = _rotationVector;

        // �㉺���_
        float _cameraUpDownRotation = Input.GetAxis("Mouse Y") * lookSensitivity;
        cameraUpAndDownRotation = _cameraUpDownRotation;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (fpsCamera != null)
        {
            currentCameraUpAndDownRotation -= cameraUpAndDownRotation;                                  // �J�[�\���̕���������]�l�ɂ���
            currentCameraUpAndDownRotation = Mathf.Clamp(currentCameraUpAndDownRotation, -80, 80);      // �l�𐧌�
            fpsCamera.transform.localEulerAngles = new Vector3(currentCameraUpAndDownRotation, 0, 0);   // X���ɂ̂݃J��������]������
        }
    }

    void Move()   // �����ړ�
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * moveX;
        Vector3 moveVertical = transform.forward * moveZ;

        speedNow = IsDash() ? speedDash : speedWalk;   // ���x�̐ݒ�

        Vector3 movementVelocity = (moveHorizontal + moveVertical) * speedNow;
        velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);

        rb.velocity = velocity;
    }

    private bool IsDash()   // �_�b�V����Ԃ̔���
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
