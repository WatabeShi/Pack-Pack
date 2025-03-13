using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    private static int life = 3;

    [Header("ライフ画像"), SerializeField]
    private GameObject[] lifeImgs;

    public void InitLife()   // ライフ初期化
    {
        life = 3;
    }

    public void Damage()   // ライフ減少
    {
        life--;
    }

    public void DispLife()   // ライフ表示
    {
        for (int i = 0; i < lifeImgs.Length; i++)
        {
            bool isActive = true;

            if (i >= life) isActive = false;

            lifeImgs[i].gameObject.SetActive(isActive);
        }
    }

    public bool IsGameOver()   // ゲームオーバーの判定
    {
        return life <= 0;
    }
}
