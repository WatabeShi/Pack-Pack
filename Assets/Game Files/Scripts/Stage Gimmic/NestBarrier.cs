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
        // “G‚ªG‚ê‚½ê‡A’Ê‚ê‚é‚æ‚¤‚É‚·‚é
        if (collision.gameObject.CompareTag("Enemy")) myCollider.isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // “G‚ª‚·‚è”²‚¯‚½ê‡A’Ê‚ê‚È‚¢‚æ‚¤‚É‚·‚é
        if (other.gameObject.CompareTag("Enemy")) myCollider.isTrigger = false;
    }
}
