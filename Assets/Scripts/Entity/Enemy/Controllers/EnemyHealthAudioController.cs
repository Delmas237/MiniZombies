using PlayerLib;
using UnityEngine;

public class EnemyHealthAudioController : HealthAudioController
{
    public override void OnDeath()
    {
        float randomRange = 0.15f;
        _deathSound.pitch = Random.Range(_deathSound.pitch - randomRange, _deathSound.pitch + randomRange);
        _deathSound.Play();
    }
}
