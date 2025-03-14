using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownPower : MonoBehaviour
{
    [Header("リスポーンさせる場所"), SerializeField]
    private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.transform.position = respawnPoint.position;
        }
    }
}
