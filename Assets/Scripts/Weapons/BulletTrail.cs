using System.Collections;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;

    private ParticleSystem _particleSystem;
    public ParticleSystem ParticleSystem => _particleSystem;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitCor());
    }

    private IEnumerator WaitCor()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitWhile(() => _audioSource.isPlaying || !_particleSystem.isStopped);
        gameObject.SetActive(false);
    }
}
