using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestBarrier : MonoBehaviour
{
    private Collider myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 敵が触れた場合、通れるようにする
        if (collision.gameObject.CompareTag("Enemy")) myCollider.isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // 敵がすり抜けた場合、通れないようにする
        if (other.gameObject.CompareTag("Enemy")) myCollider.isTrigger = false;
    }
}
