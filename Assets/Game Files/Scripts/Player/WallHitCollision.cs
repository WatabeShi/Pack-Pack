using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHitCollision : MonoBehaviour
{
    private bool isHit = false;   // �ǂɐG��Ă��邩�̔���

    private bool IsHitWall(Collider _other)   // �ǂɐG�ꂽ������
    {
        return _other.gameObject.CompareTag("Wall");
    }

    private void OnTriggerStay(Collider other)
    {
        // �ǂɐG�ꂽ���̏�����
        if (!IsHitWall(other)) return;

        isHit = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // �ǂɗ��ꂽ���̏�����
        if (!IsHitWall(other)) return;

        isHit = false;
    }

    public bool IsHit
    {
        get { return isHit; }
    }
}
