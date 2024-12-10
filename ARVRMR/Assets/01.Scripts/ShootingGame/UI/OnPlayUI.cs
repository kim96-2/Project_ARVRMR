using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 플레이 중에 나오는 UI 컴포넌트
/// </summary>
public class OnPlayUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timerText;

    [Space(15f)]
    [SerializeField] GameObject GameOverUI;

    void Awake()
    {
        ShootingGameManager.Instance.OnScoreChange += SetScoreText;
        ShootingGameManager.Instance.OnGameEnd += GameEnd;
    }

    private void Update()
    {
        timerText.text = "Time Left: " + ShootingGameManager.Instance.TimeLeft.ToString("F0");
    }

    public void SetScoreText(float score)
    {
        scoreText.text = "Score:" + score.ToString(); 
    }

    public void GameEnd()
    {
        GameOverUI.SetActive(true);
        this.gameObject.SetActive(false);

        Debug.Log("Turn On Game Over UI");
    }
}
