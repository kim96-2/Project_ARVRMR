using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.XR.Interaction.Toolkit;

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

    [Header("Haptic")]
    [SerializeField] float hapticIntensity = 2f;
    [SerializeField] float hapticDuration = 0.2f;

    [Serializable]
    class AnimationValue
    {
        public Vector3 pos;
        public float rot;
    }
    [Header("Animation")]
    [SerializeField] AnimationValue startAnimValue;
    [SerializeField] AnimationValue endAnimValue;
    [SerializeField] float animDuration;
    [SerializeField] AnimationCurve animCurve;

    [Space(5f)]
    [SerializeField] Transform animTarget;


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

    public void Shoot(ActivateEventArgs args)
    {
        //반동 애니메이션 실행
        StartAnim();

        //파티클 생성
        shootFlashParticle.Emit(flashParticleCount);
        
        //라이트 애니메이션
        if (flashLight)
        {
            if(lightCoroutine!=null) StopCoroutine(lightCoroutine);

            lightCoroutine = StartCoroutine(LigthAnim());
        }

        //햅틱 적용
        if(args.interactorObject is XRBaseControllerInteractor interactor)
        {
            TriggerHaptic(interactor.xrController);
        }
    }

    #region Light
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
    #endregion

    #region Haptic
    void TriggerHaptic(XRBaseController controller)
    {
        if (hapticIntensity > 0f)
        {
            controller.SendHapticImpulse(hapticIntensity, hapticDuration);
        }
    }

    #endregion

    #region Animation
    
    Coroutine AnimCoroutine;

    /// <summary>
    /// 무기 반동 애니메이션
    /// </summary>
    void StartAnim()
    {
        if(AnimCoroutine!= null)
        {
            StopCoroutine(AnimCoroutine);
        }

        AnimCoroutine = StartCoroutine(AnimationPlay());
    }

    IEnumerator AnimationPlay()
    {
        float _time = 0f;

        while(_time < animDuration)
        {
            Vector3 _pos = Vector3.Lerp(startAnimValue.pos, endAnimValue.pos, _time / animDuration);
            
            float _rot = Mathf.Lerp(startAnimValue.rot, endAnimValue.rot, _time / animDuration);
            Quaternion _rotEuler = Quaternion.Euler(0, 0, _rot);

            animTarget.localPosition = _pos;
            animTarget.localRotation = _rotEuler;

            _time += Time.deltaTime;

            //Debug.Log(_rot);

            yield return null;
        }

        animTarget.localPosition = Vector3.zero;
        animTarget.localRotation = Quaternion.identity;

        AnimCoroutine = null;
    }
    #endregion
}
