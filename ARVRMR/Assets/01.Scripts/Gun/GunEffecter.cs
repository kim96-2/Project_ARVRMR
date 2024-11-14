using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GunEffecter : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] ParticleSystem shootFlashParticle;
    [SerializeField] int flashParticleCount = 5;

    [Header("Light")]
    [SerializeField] Light flashLight;
    [SerializeField] float flashLightDuration = 0.5f;
    [SerializeField] AnimationCurve flashLightCurve;

    float flashLightMultipler;
    float defaultFlashLightValue;

    GunShooter shooter;

    // Start is called before the first frame update
    void Start()
    {    
        flashLightMultipler = 0f;
        defaultFlashLightValue = flashLight.intensity;
        flashLight.intensity = defaultFlashLightValue * flashLightMultipler;
        flashLight.enabled = false;

        shooter = GetComponentInParent<GunShooter>();
        if(shooter ==null) TryGetComponent<GunShooter>(out shooter);

        if (shooter != null)
        {
            shooter.shootAction += Shoot;
        }
    }

    private void OnDisable()
    {
        if (shooter != null)
        {
            shooter.shootAction -= Shoot;
        }
    }

    public void Shoot()
    {
        //파티클 생성
        shootFlashParticle.Emit(flashParticleCount);

        //라이트 애니메이션
        if (flashLight)
        {
            if(lightCoroutine!=null) StopCoroutine(lightCoroutine);

            lightCoroutine = StartCoroutine(LigthAnim());
        }
    }

    Coroutine lightCoroutine;
    IEnumerator LigthAnim()
    {
        flashLight.enabled = true;

        float _time = 0f;

        while (_time < flashLightDuration)
        {
            flashLightMultipler = flashLightCurve.Evaluate(_time / flashLightDuration);

            flashLight.intensity = defaultFlashLightValue * flashLightMultipler;

            _time += Time.deltaTime;

            yield return null;
        }

        flashLightMultipler = 0f;
        flashLight.intensity = defaultFlashLightValue * flashLightMultipler;
        flashLight.enabled = false;

    }
}
