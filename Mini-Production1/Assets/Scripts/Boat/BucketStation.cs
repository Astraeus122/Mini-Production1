using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BucketStation : MonoBehaviour
{
    [SerializeField]
    private BoatMovement boat = null;

    [SerializeField, Tooltip("Health gain per second.")]
    private float bailRate = 0.4f;

    [SerializeField]
    private ParticleSystem bailParticles;

    private Animator animator;
    private int animBailHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animBailHash = Animator.StringToHash("Bailing");
    }

    private void Update()
    {
        Debug.Log("Chilly");
        boat.TakeDamage(-bailRate * Time.deltaTime);

        //TODO maybe check boat max health and disable this when no more bailing to do?
    }

    public void AnimSetBail(bool val)
    {
        animator.SetBool(animBailHash, val);
    }

    public void Bail()
    {
        bailParticles.Play();
    }
}
