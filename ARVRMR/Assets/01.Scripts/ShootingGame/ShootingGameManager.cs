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

    [Space(15f)]
    [SerializeField] BoxCollider rangeCollider;
    [SerializeField] float yOffset;
    
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
        CreateTarget();
    }

    public void CreateTarget()
    {
        //타겟 생성 위치 계산
        Vector3 spawnPosCenter = rangeCollider.transform.position;

        float range_x = rangeCollider.bounds.size.x;
        float range_z = rangeCollider.bounds.size.z;

        range_x = Random.Range(range_x / 2f * -1, range_x / 2f);
        range_z = Random.Range(range_z / 2f * -1, range_z / 2f);

        Vector3 spawnPos = new Vector3(range_x,0,range_z) + spawnPosCenter;
        spawnPos.y = yOffset;

        //타겟 생성
        Instantiate(target, spawnPos, target.transform.rotation);


    }

    public void TargetDestroyed(bool headShot)
    {
        CreateTarget();
    }


    
}
