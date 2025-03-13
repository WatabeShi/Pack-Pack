using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    private bool isInArea = false;   // �v���C���[���G���A���ɂ��邩

    private bool CheckPlayer(Collider _other)   // �v���C���[�����邩����
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
