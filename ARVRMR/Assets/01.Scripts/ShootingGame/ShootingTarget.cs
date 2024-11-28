using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    [SerializeField] BoxCollider hitCollider;
    [SerializeField, Range(0f, 1f)] float headShotRange = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //테스트
        //if (Input.GetMouseButtonDown(0)) Hit(Vector3.zero);
    }

    public void Hit(Vector3 hitPos)
    {
        Debug.Log("과녁 맞음");

        //해드샷 여부 계산
        Vector3 targetCenter = transform.TransformPoint(hitCollider.center);

        float lengthFromCenter = Vector3.Magnitude(targetCenter - hitPos);

        float targetSize = hitCollider.bounds.size.x / 2f;

        bool headShot = targetSize * headShotRange > lengthFromCenter ? true : false;

        //맞았다고 게임매니져에게 전달
        ShootingGameManager.Instance.TargetDestroyed(headShot);

        Destroy(this.gameObject);
    }
}
