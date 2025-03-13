using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0;

    private float speedX = 0;
    private float speedZ = 0;

    private enum MovePattern
    {
        none,
        stop,
        forward,
        back,
        left,
        right
    };
    private MovePattern mPattern;       // 現在移動している方向
    private MovePattern mPatternSave;   // 移動する方向の保存用変数

    private bool isFirstMove = true;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //mPattern = MovePattern.forward;
        //mPatternSave = MovePattern.none;

        //UpdateParam();
    }

    // Update is called once per frame
    void Update()
    {
        //FirstMove();
        //MoveInput();

        if(Input.GetKey(KeyCode.W))
        {
            speedX = 0;
            speedZ = 3;
        }
        if (Input.GetKey(KeyCode.S))
        {
            speedX = 0;
            speedZ = -3;
        }
        if (Input.GetKey(KeyCode.A))
        {
            speedX = -3;
            speedZ = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            speedX = 3;
            speedZ = 0;
        }

        rb.velocity = new Vector3(speedX, 0, speedZ);
        //this.transform.position += new Vector3(speedX, 0, speedZ) * Time.deltaTime;
    }

    private void FirstMove()
    {
        if (!isFirstMove) return;

        if (Input.GetKey(KeyCode.W))
        {
            mPattern = MovePattern.forward;
            UpdateParam();

            isFirstMove = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            mPattern = MovePattern.back;
            UpdateParam();

            isFirstMove = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            mPattern = MovePattern.left;
            UpdateParam();

            isFirstMove = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            mPattern = MovePattern.right;
            UpdateParam();

            isFirstMove = false;
        }
    }

    private void MoveInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            mPatternSave = MovePattern.forward;
            UpdateParam();
        }
        if (Input.GetKey(KeyCode.S))
        {
            mPatternSave = MovePattern.back;
            UpdateParam();
        }
        if (Input.GetKey(KeyCode.A))
        {
            mPatternSave = MovePattern.left;
            UpdateParam();
        }
        if (Input.GetKey(KeyCode.D))
        {
            mPatternSave = MovePattern.right;
            UpdateParam();
        }
    }

    private void UpdateParam()
    {
        switch (mPattern)
        {
            case MovePattern.stop:

                speedX = 0;
                speedZ = 0;
                break;
            case MovePattern.forward:

                speedX = 0;
                speedZ = moveSpeed;
                break;
            case MovePattern.back:

                speedX = 0;
                speedZ = -moveSpeed;
                break;
            case MovePattern.left:

                speedX = -moveSpeed;
                speedZ = 0;
                break;
            case MovePattern.right:

                speedX = moveSpeed;
                speedZ = 0;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Wall"))   // 壁に当たったら
        //{
        //    mPattern = MovePattern.stop;
        //    UpdateParam();
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Corner")) return;
        var pl = other.gameObject.transform.position - this.transform.position;

        print(pl.magnitude);   // 当たり判定の中心からの距離を表示

        if (pl.magnitude > 0.25f) return;   // 当たり判定の中心からの距離から

        mPattern = mPatternSave;
        UpdateParam();
    }
}
