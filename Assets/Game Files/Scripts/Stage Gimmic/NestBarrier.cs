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
        // �G���G�ꂽ�ꍇ�A�ʂ��悤�ɂ���
        if (collision.gameObject.CompareTag("Enemy")) myCollider.isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // �G�����蔲�����ꍇ�A�ʂ�Ȃ��悤�ɂ���
        if (other.gameObject.CompareTag("Enemy")) myCollider.isTrigger = false;
    }
}
