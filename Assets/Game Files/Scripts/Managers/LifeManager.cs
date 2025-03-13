using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    private static int life = 3;

    [Header("���C�t�摜"), SerializeField]
    private GameObject[] lifeImgs;

    public void InitLife()   // ���C�t������
    {
        life = 3;
    }

    public void Damage()   // ���C�t����
    {
        life--;
    }

    public void DispLife()   // ���C�t�\��
    {
        for (int i = 0; i < lifeImgs.Length; i++)
        {
            bool isActive = true;

            if (i >= life) isActive = false;

            lifeImgs[i].gameObject.SetActive(isActive);
        }
    }

    public bool IsGameOver()   // �Q�[���I�[�o�[�̔���
    {
        return life <= 0;
    }
}
