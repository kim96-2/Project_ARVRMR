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

    void Awake()
    {
        ShootingGameManager.Instance.OnScoreChange += SetScoreText;
    }

    public void SetScoreText(float score)
    {
        scoreText.text = "Score:" + score.ToString(); 
    }
}
