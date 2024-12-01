using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowlingScoreText : MonoBehaviour
{
    [SerializeField] private GameObject totalScore;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }

        totalScore.GetComponent<TextMeshProUGUI>().text = "Total: 0";
    }

    public void SetBowlingScoreUI(List<BowlingFrameScore> scores) 
    {
        int culmulativeScore = 0;

        for (int i = 0; i < scores.Count; i++)
        {
            string f;
            if (scores[i].IsStrike()) f = "X";
            else if (scores[i].GetFirstTry() == -1) break;
            else f = scores[i].GetFirstTry().ToString();
            transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = f;

            string s;
            if (f == "X") s = "";
            else if (scores[i].IsSpare()) s = "/";
            else if (scores[i].GetSecondTry() == -1) break;
            else s = scores[i].GetSecondTry().ToString();
            transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = s;

            if (scores[i].GetCumulativeScore(culmulativeScore) == -1) continue;
            culmulativeScore = scores[i].GetCumulativeScore(culmulativeScore);
            transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = culmulativeScore.ToString();
        }

        totalScore.GetComponent<TextMeshProUGUI>().text = "Total: " + culmulativeScore;
    }
}
