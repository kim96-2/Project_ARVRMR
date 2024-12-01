using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingSystem : MonoBehaviour
{
    [SerializeField] private GameObject bowlingBall;
    [SerializeField] private GameObject bowlingPins;
    [SerializeField] private GameObject bowlingShutter;

    [SerializeField] private Vector3[] bowlingPinPositions;

    [SerializeField] private List<BowlingFrameScore> bowlingScoreList = new();
    [SerializeField] private GameObject bowlingScoreText;

    float BALL_THRESHOLD = -5.0f;
    float PIN_THRESHOLD = -0.01f;
    bool[] pinStates = new bool[10];

    private bool isProgressed = false;

    float MAX_FRAME_NUMBER = 5;

    // Start is called before the first frame update
    void Start()
    {
        bowlingScoreList.Add(new BowlingFrameScore(1));
        for (int i = 0; i < pinStates.Length; i++) pinStates[i] = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bowlingBall.transform.position.y < BALL_THRESHOLD) 
        {
            bowlingBall.transform.position = Vector3.up;
            StopAllCoroutines();
            StartCoroutine(WaitPinsStable());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            bowlingBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bowlingBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            bowlingBall.transform.position = new Vector3(0.2f, 0.3f, 1.99f);
            bowlingBall.GetComponent<BowlingBall>().TestBowlingBall();
        }
    }
    IEnumerator WaitPinsStable() 
    {
        float limit = 10;
        while (limit > 0) 
        {
            yield return new WaitForSeconds(Time.deltaTime);
            limit -= Time.deltaTime;

            float velocities = 0;
            for (int i = 0; i < bowlingPinPositions.Length; i++)
            {
                if (bowlingPins.transform.GetChild(i).transform.position.y < BALL_THRESHOLD) continue;
                velocities += bowlingPins.transform.GetChild(i).GetComponent<Rigidbody>().velocity.magnitude;
                velocities += bowlingPins.transform.GetChild(i).GetComponent<Rigidbody>().angularVelocity.magnitude;
            }

            if (velocities < 0.1f) break;
        }
        ProgressBowlingSet();
    }

    public void ProgressBowlingSet() 
    {
        if (isProgressed) return;
        isProgressed = true;
        StartCoroutine(ShutterAnimation());
        CheckPins();
        CalculateScore();
        StartCoroutine(ResetAnimation(new WaitForSeconds(2.0f)));
    }

    private void CheckPins()
    {
        int cnt = 0;
        for (int i = 0; i < pinStates.Length; i++) 
        {
            if (bowlingPins.transform.GetChild(i).transform.localPosition.y < PIN_THRESHOLD && pinStates[i] == false) 
            {
                cnt++;
                pinStates[i] = true;
            }
        }

        int tryNum = 1;
        if (bowlingScoreList[bowlingScoreList.Count - 1].IsSpareRound())
        {
            tryNum = 2;
        }

        bowlingScoreList[bowlingScoreList.Count - 1].AddScore(tryNum, cnt);
    }
    private void CalculateScore()
    {
        for (int i = 0; i < bowlingScoreList.Count; i++) 
        {
            if (i == bowlingScoreList.Count - 1)
            {
                if (i == MAX_FRAME_NUMBER - 1)
                {
                    bowlingScoreList[i].ConfirmBonusScore(0, 0);
                }
            }
            else
            {
                bowlingScoreList[i].ConfirmBonusScore(bowlingScoreList[i + 1].GetFirstTry(), bowlingScoreList[i + 1].GetSecondTry());
            }
        }

        if (bowlingScoreList[bowlingScoreList.Count - 1].IsFrameEnd())
        {
            if (bowlingScoreList.Count == MAX_FRAME_NUMBER)
            {
                //end bowling game
            }
            else
            {
                bowlingScoreList.Add(new BowlingFrameScore(bowlingScoreList.Count + 1));
                for (int i = 0; i < pinStates.Length; i++) pinStates[i] = false;
            }
        }

        bowlingScoreText.GetComponent<BowlingScoreText>().SetBowlingScoreUI(bowlingScoreList);
    }
    IEnumerator ResetAnimation(WaitForSeconds wait)
    {
        yield return wait;
        int a=0, b = 0;
        for (int i = 0; i < bowlingPinPositions.Length; i++)
        {
            bowlingPins.transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
            bowlingPins.transform.GetChild(i).GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            bowlingPins.transform.GetChild(i).GetComponent<Rigidbody>().MovePosition(bowlingPins.transform.position + bowlingPinPositions[i]);
            bowlingPins.transform.GetChild(i).GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(new Vector3(-90, 0, 0)));
            //BowlingPins.transform.GetChild(i).transform.SetLocalPositionAndRotation(bowlingPinPositions[i], Quaternion.Euler(new Vector3(-90, 0, 0)));
            a++;
            if (pinStates[i])
            {
                b++;
                Vector3 tmpPos = bowlingPins.transform.GetChild(i).localPosition;
                tmpPos.z = 10;
                bowlingPins.transform.GetChild(i).transform.SetLocalPositionAndRotation(tmpPos, Quaternion.Euler(new Vector3(-90, 0, 0)));
            }
        }

        isShutterMove = true;
        isProgressed = false;

        yield break;
    }

    bool isShutterMove = true;
    IEnumerator ShutterAnimation()
    {
        bool isDown = true;
        int dir = -1;
        float speed = 0.9f;
        float downYPosition = 0.25f;
        float upYPosition = 0.7f;

        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (!isShutterMove)
            {
                continue;
            }
            Vector3 tmpPos = bowlingShutter.transform.localPosition;
            tmpPos.y += dir * speed * Time.deltaTime;
            bowlingShutter.transform.localPosition = tmpPos;

            if (tmpPos.y < downYPosition)
            {
                isShutterMove = false;
                yield return new WaitForSeconds(1.0f);
                isDown = !isDown;
                dir = -dir;
            }

            if (tmpPos.y > upYPosition) 
            {
                yield break;
            }
        }

    }
}


public class BowlingFrameScore 
{
    int frameNumber;
    int firstTry;
    int secondTry;

    bool isStrike;
    bool isSpare;

    int frameScore;

    public int GetFirstTry() => firstTry;
    public int GetSecondTry() => secondTry;
    public bool IsStrike() => isStrike;
    public bool IsSpare() => isSpare;

    public BowlingFrameScore(int setNum) 
    {
        frameNumber = setNum;
        firstTry = -1;
        secondTry = -1;

        isStrike = false;
        isSpare = false;

        frameScore = -1;
    }
    public void AddScore(int tryNum, int pinCount) 
    {
        if (tryNum == 1 && firstTry == -1)
        {
            firstTry = pinCount;
            if (firstTry == 10) 
            {
                secondTry = 0;
                isStrike = true;
            }
        }
        else if (tryNum == 2 && secondTry == -1)
        {
            secondTry = pinCount;
            if (firstTry + secondTry == 10)
            {
                isSpare = true;
            }

            if (!isSpare) frameScore = firstTry + secondTry;
        }
        else 
        {
            Debug.LogError("Wrong Score Input: " + frameNumber + " / " + tryNum);
        }
    }
    public void ConfirmBonusScore(int postFirstTry, int postSecondTry) 
    {
        if (frameScore != -1) return;

        if (isSpare && postFirstTry != -1) frameScore = firstTry + secondTry + postFirstTry;
        if (isStrike && postSecondTry != -1) frameScore = firstTry + secondTry + postFirstTry + postSecondTry;
    }

    public int GetCumulativeScore(int prevScore)
    {
        if (frameScore == -1) return -1;
        else return prevScore + frameScore;
    }

    public bool IsSpareRound()
    {
        return firstTry != -1;
    }
    public bool IsFrameEnd() 
    {
        return secondTry != -1;
    }
}
