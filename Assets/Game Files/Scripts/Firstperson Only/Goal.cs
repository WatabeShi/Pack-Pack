using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        ScoreRanking.instance.RankInFPS(ScoreManager.scoreNow);

        BGMSource.instance.StopBGM();
        SceneManager.LoadScene("Gameclear");
    }
}
