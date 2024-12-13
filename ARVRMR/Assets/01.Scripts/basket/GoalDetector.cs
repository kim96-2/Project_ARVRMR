using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalDetector : MonoBehaviour
{
    public int score = 0;  // 점수 기록
    public TextMeshProUGUI scoreText;  // UI 텍스트 참조

    void Start()
    {
        UpdateScoreText();  // 초기 점수 텍스트 설정
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered by: " + other.name);  // 어떤 오브젝트가 트리거를 통과했는지 출력

        if (other.CompareTag("Basketball"))
        {
            score++;
            UpdateScoreText();  // 점수 업데이트
            Debug.Log("Goal! Current Score: " + score);
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}