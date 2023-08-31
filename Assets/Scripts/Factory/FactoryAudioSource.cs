using UnityEngine;

namespace Factory
{
    public class FactoryAudioSource : FactoryBase<AudioSource>
    {
        public FactoryAudioSource(AudioSource prefab, Transform parent) : base(prefab, parent) { }

        public override void ReconstructToDefault(AudioSource audioSource) { }

        public override void Construct(AudioSource audioSource)
        {
            audioSource.Play();
        }
    }
}
