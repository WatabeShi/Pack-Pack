using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveFPS : MonoBehaviour
{
    [Header("歩行速度"), SerializeField]
    private float speedWalk = 0;
    [Header("ダッシュ速度"), SerializeField]
    private float speedDash = 0;
    private float speedNow = 0;   // 現在の速度

    [Header("カメラ感度"), SerializeField]
    private float lookSensitivity = 5f;
    [Header("プレイヤーの視点となるカメラ"), SerializeField]
    GameObject fpsCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    private float cameraUpAndDownRotation = 0f;         //視点上下用の変数
    private float currentCameraUpAndDownRotation = 0f;  //視点上下用の変数

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

    void CameraControl()   // カメラ操作
    {
        // 左右視点
        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _rotationVector = new Vector3(0, _yRotation, 0) * lookSensitivity;
        rotation = _rotationVector;

        // 上下視点
        float _cameraUpDownRotation = Input.GetAxis("Mouse Y") * lookSensitivity;
        cameraUpAndDownRotation = _cameraUpDownRotation;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (fpsCamera != null)
        {
            currentCameraUpAndDownRotation -= cameraUpAndDownRotation;                                  // カーソルの方を向く回転値にする
            currentCameraUpAndDownRotation = Mathf.Clamp(currentCameraUpAndDownRotation, -80, 80);      // 値を制限
            fpsCamera.transform.localEulerAngles = new Vector3(currentCameraUpAndDownRotation, 0, 0);   // X軸にのみカメラを回転させる
        }
    }

    void Move()   // 水平移動
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * moveX;
        Vector3 moveVertical = transform.forward * moveZ;

        speedNow = IsDash() ? speedDash : speedWalk;   // 速度の設定

        Vector3 movementVelocity = (moveHorizontal + moveVertical) * speedNow;
        velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);

        rb.velocity = velocity;
    }

    private bool IsDash()   // ダッシュ状態の判定
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
