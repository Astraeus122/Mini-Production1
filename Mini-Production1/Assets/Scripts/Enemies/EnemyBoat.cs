using UnityEngine;

public class EnemyBoat : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem bubbles = null;

    [SerializeField]
    private ParticleSystem splashes = null;

    [SerializeField]
    private BoatMovement boat;

    public void StartBubbling()
    {
        bubbles.Play();
    }

    public void StopBubbling()
    {
        bubbles.Stop();
    }

    public void Splash()
    {
        splashes.Play();
    }

    public void StartControl()
    {
        boat.enabled = true;
    }

    private void Update()
    {
        boat.SteeringInput = 1;
    }
}
