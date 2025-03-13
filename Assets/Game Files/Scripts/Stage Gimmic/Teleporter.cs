using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("移動させるポイント"), SerializeField]
    private Transform tereportPoint;

    [Header("テレポートSE"), SerializeField]
    private AudioClip tpSE;

    private bool IsEntity(Collider _other)   // エンティティかチェックする
    {
        if (_other.gameObject.CompareTag("Player") || _other.gameObject.CompareTag("Enemy")) return true;
        else return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsEntity(other)) return;

        var tpPos = new Vector3(tereportPoint.position.x, other.transform.position.y, tereportPoint.position.z);
        other.gameObject.transform.position = tpPos;   // Teleport, now.
        SESource.instance.PlaySE(tpSE);
    }
}
