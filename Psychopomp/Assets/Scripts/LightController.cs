using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{

    Light spotLight;
    [SerializeField] Light boatLight;
    [SerializeField] Light globalLight;
    [SerializeField] Volume volume;

    public static LightController Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void setLightToHandle(Light light)
    {
        spotLight = light;
        boatLight = GameObject.FindGameObjectWithTag("Boat").transform.Find("BoatLight").GetComponent<Light>();
    }

    public void turnOn()
    {
        float duration = 1f;
        StartCoroutine(IncreaseToTargetCoroutine(spotLight, 15f, duration));
        StartCoroutine(IncreaseToTargetCoroutine(volume, 1f, duration));
        StartCoroutine(DecreaseToTargetCoroutine(globalLight, 0f, duration));
        StartCoroutine(IncreaseToTargetCoroutine(boatLight, 2f, duration));
    }

    public void turnOff()
    {
        float duration = 1f;
        StartCoroutine(DecreaseToTargetCoroutine(spotLight, 0f, duration));
        StartCoroutine(DecreaseToTargetCoroutine(volume, 0f, duration));
        StartCoroutine(IncreaseToTargetCoroutine(globalLight, 1f, duration));
        StartCoroutine(DecreaseToTargetCoroutine(boatLight, 0f, duration));
    }


    IEnumerator IncreaseToTargetCoroutine(Light light, float targetIntensity, float duration)
    {
        float currentIntensity = light.intensity;
        float incrementAmount = (targetIntensity - currentIntensity) * Time.deltaTime / duration;

        while (currentIntensity < targetIntensity)
        {
            currentIntensity += incrementAmount;
            light.intensity = currentIntensity;
            yield return null;
        }
        light.intensity = targetIntensity;
    }

    IEnumerator DecreaseToTargetCoroutine(Light light, float targetIntensity, float duration)
    {
        float currentIntensity = light.intensity;
        float decrementAmount = (currentIntensity - targetIntensity) * Time.deltaTime / duration;

        while (currentIntensity > targetIntensity)
        {
            currentIntensity -= decrementAmount;
            light.intensity = currentIntensity;
            yield return null;
        }
        light.intensity = targetIntensity;
    }

    IEnumerator IncreaseToTargetCoroutine(Volume volume, float targetIntensity, float duration)
    {
        float currentIntensity = volume.weight;
        float incrementAmount = (targetIntensity - currentIntensity) * Time.deltaTime / duration;

        while (currentIntensity < targetIntensity)
        {
            currentIntensity += incrementAmount;
            volume.weight = currentIntensity;
            yield return null;
        }
        volume.weight = targetIntensity;
    }

    IEnumerator DecreaseToTargetCoroutine(Volume volume, float targetIntensity, float duration)
    {
        float currentIntensity = volume.weight;
        float decrementAmount = (currentIntensity - targetIntensity) * Time.deltaTime / duration;

        while (currentIntensity > targetIntensity)
        {
            currentIntensity -= decrementAmount;
            volume.weight = currentIntensity;
            yield return null;
        }
        volume.weight = targetIntensity;
    }
}
