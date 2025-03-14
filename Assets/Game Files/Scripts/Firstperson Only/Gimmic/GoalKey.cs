using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKey : MonoBehaviour
{
    [Header("入手した時のSE"), SerializeField]
    private AudioClip getSE;

    [Header("回転速度"), SerializeField]
    private float rotateSpeed = 0;

    private void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);   // 鍵の回転
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        SESource.instance.PlaySE(getSE);
    }
}
