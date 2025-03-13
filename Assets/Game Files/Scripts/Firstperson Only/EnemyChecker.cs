using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    private bool isInArea = false;   // プレイヤーがエリア内にいるか

    private bool CheckPlayer(Collider _other)   // プレイヤーがいるか判定
    {
        return _other.gameObject.CompareTag("Enemy");
    }

    private void OnTriggerStay(Collider other)
    {
        if (CheckPlayer(other)) isInArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckPlayer(other)) isInArea = false;
    }

    public bool IsInArea
    {
        get { return isInArea; }
    }
}
