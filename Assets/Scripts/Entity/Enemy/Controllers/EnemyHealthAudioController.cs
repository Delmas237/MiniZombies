using PlayerLib;
using UnityEngine;

public class EnemyHealthAudioController : HealthAudioController
{
    public override void OnDeath()
    {
        deathSound.pitch = Random.Range(deathSound.pitch - 0.15f, deathSound.pitch + 0.15f);
        deathSound.Play();
    }
}
