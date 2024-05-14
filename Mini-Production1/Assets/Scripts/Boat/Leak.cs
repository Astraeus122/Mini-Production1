using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Interactable))]
public class Leak : MonoBehaviour
{
    [SerializeField]
    private float leakGainDuration;

    [SerializeField]
    private float leakRepairDuration;

    [SerializeField]
    private AnimationCurve scaleByStrength;

    [SerializeField]
    private AnimationCurve emissionFactorByStrength;

    [SerializeField]
    private ParticleSystem leakParticles;

    [SerializeField]
    private ParticleSystem leakStartParticles;

    [SerializeField]
    private Transform repairProgressObject = null;

    [SerializeField]
    private Image repairProgressImage = null;

    //[SerializeField] // set auto in start now as the beginning localposition
    private Vector3 progressUiOffset;

    private float emissionAmount;
    private Interactable interactable;

    private float backingLeakStrength;
    public float LeakStrength
    {
        get
        {
            return backingLeakStrength;
        }
        set
        {
            backingLeakStrength = Mathf.Clamp01(value);
            SetVFX();
        }
    }

    private List<Interactor> Repairers = new List<Interactor>();


    private void Awake()
    {
        interactable = GetComponent<Interactable>();

        emissionAmount = leakParticles.emission.rateOverTimeMultiplier;

        interactable.enabled = enabled;

        progressUiOffset = repairProgressObject.localPosition;
    }

    private void OnEnable()
    {
        leakStartParticles.Play();
        LeakStrength = 1;

        repairProgressObject.gameObject.SetActive(true);
        interactable.enabled = true;
    }
    private void OnDisable()
    {
        LeakStrength = 0;

        repairProgressObject.gameObject.SetActive(false);
        interactable.enabled = false;
    }

    public void UpdateLeakRepairDuration(int repairSpeedLevel)
    {
        float decreasePercentage = 0.1f; // 10% decrease per level
        leakRepairDuration = leakRepairDuration * (1 - (repairSpeedLevel * decreasePercentage));
    }

    public void AddRepairer(Interactor i)
    {
        Repairers.Add(i);
    }

    public void RemoveRepairer(Interactor i)
    {
        Repairers.Remove(i);
    }

    private void Update()
    {
        // temp fix to keep the progress UI from rotating with our leak rotation since its a child and inherits rotation
        repairProgressObject.position = transform.position + progressUiOffset;

        if (Repairers.Count > 0) // is repairing
            LeakStrength -= Time.deltaTime / leakRepairDuration * Repairers.Count; // * count to allow stacking
        else
            LeakStrength += Time.deltaTime / leakGainDuration;

        repairProgressImage.fillAmount = 1 - LeakStrength;

        if (LeakStrength <= 0) enabled = false;
    }

    private void SetVFX()
    {
        if (LeakStrength == 0)
        {
            if (leakParticles.isPlaying) leakParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        else if (!leakParticles.isPlaying) leakParticles.Play();

        var emissionSettings = leakParticles.emission;
        emissionSettings.rateOverTime = emissionAmount * emissionFactorByStrength.Evaluate(LeakStrength);

        leakParticles.transform.localScale = Vector3.one * scaleByStrength.Evaluate(LeakStrength);
    }
    public void ReactivateLeak(float strengthIncrease)
    {
        if (!enabled) // If the leak is disabled, we enable it
        {
            enabled = true;
            OnEnable();  // Call OnEnable to reset the necessary components and visuals
        }
        LeakStrength += strengthIncrease;  // Increase leak strength
        LeakStrength = Mathf.Min(LeakStrength, 1);  // Ensure it does not exceed 1
    }

    public void UpdateLeakRepairDuration()
    {
        float decreasePercentage = 0.1f;
        leakRepairDuration *= (1 - decreasePercentage);
    }
}
