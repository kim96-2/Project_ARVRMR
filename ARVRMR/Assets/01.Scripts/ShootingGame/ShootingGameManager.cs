using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 총게임 매니져
/// </summary>
public class ShootingGameManager : MonoBehaviour
{
    /// <summary>
    /// 싱글톤으로 제작
    /// </summary>
    private static ShootingGameManager _instance;
    public static ShootingGameManager Instance {  get { return _instance; } }

    [SerializeField] GameObject target;
    GameObject currentTargetObject;

    [Space(15f)]
    [SerializeField] BoxCollider rangeCollider;
    [SerializeField] float yOffset;

    [SerializeField] Transform targetPoss;

    [Header("Score Setting")]
    [SerializeField] float hitScore = 10;
    [SerializeField] float headShotScore = 20;

    public event Action<float> OnScoreChange;

    private float _score;
    public float Score { 
        get {
            return _score; 
        }
        private set
        {
            _score = value;

            OnScoreChange?.Invoke(_score);
        }
    }

    [Header("Timer Setting")]
    [SerializeField] float gameDuration = 60;
    
    float timeLeft;
    public float TimeLeft { get => timeLeft;}

    public event Action OnGameEnd;

    private void Awake()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        //StartGame();
    }

    public void StartGame()
    {
        Score = 0;

        CreateTarget();

        if (TimerCoroutine != null) StopCoroutine(TimerCoroutine);
        TimerCoroutine = StartCoroutine(GameTimer());
    }

    public void CreateTarget()
    {
        //타겟 생성 위치 계산
        /*
        Vector3 spawnPosCenter = rangeCollider.transform.position;

        float range_x = rangeCollider.bounds.size.x;
        float range_z = rangeCollider.bounds.size.z;

        range_x = UnityEngine.Random.Range(range_x / 2f * -1, range_x / 2f);
        range_z = UnityEngine.Random.Range(range_z / 2f * -1, range_z / 2f);

        Vector3 spawnPos = new Vector3(range_x,0,range_z) + spawnPosCenter;
        spawnPos.y = yOffset;
        */

        //임의로 지정된 위치중에서 무작위로 하나 선택하는 방법으로 변경
        Vector3 spawnPos = targetPoss.GetChild(UnityEngine.Random.Range(0, targetPoss.childCount)).position;

        //타겟 생성
        currentTargetObject = Instantiate(target, spawnPos, target.transform.rotation);


    }

    public void TargetDestroyed(bool headShot)
    {
        Score += headShot ? headShotScore : hitScore;

        CreateTarget();
    }

    Coroutine TimerCoroutine;
    IEnumerator GameTimer()
    {
        timeLeft = gameDuration;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            yield return null;
        }

        timeLeft = 0f;

        TimerCoroutine = null;

        EndGame();
    }

    void EndGame()
    {
        if(currentTargetObject != null)
        {
            Destroy(currentTargetObject);
        }

        OnGameEnd?.Invoke();
    }
    
}
