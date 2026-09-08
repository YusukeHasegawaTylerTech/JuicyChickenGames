using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JuicyChickenGames
{
    public class AudioManager : MonoBehaviour
    {
        public List<AudioSource> AudioSources;

        private AudioSourcePool _audioSourcePool;

        private void Start()
        {
            _audioSourcePool = new AudioSourcePool(AudioSources);
        }

        public void PlayClip(AudioClip clip)
        {
            AudioSource audioSource = _audioSourcePool.Next();
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void PlayClip(List<AudioClip> clips)
        {
            if (clips.IsNullOrEmpty())
            {
                return;
            }

            var clip = clips.Sample();

            AudioSource audioSource = _audioSourcePool.Next();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public class AudioSourcePool
    {
        private readonly List<AudioSource> audioSources;
        private int index;

        public AudioSourcePool(List<AudioSource> audioSources)
        {
            this.audioSources = audioSources;
        }

        public AudioSource Next()
        {
            index %= audioSources.Count();
            return audioSources[index++];
        }
    }
}