using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TMP_Text finalScoreText;

    // Update is called once per frame
    void Update()
    {
        finalScoreText.text = "Final Score: " + ShootingGameManager.Instance.Score.ToString();
    }
}
