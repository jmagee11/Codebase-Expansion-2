using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace util.sound
{
    public class AudioSourceOneShot : MonoBehaviour
    {
        private static AudioSourceOneShot Current;

        public static AudioSourceOneShot GetInstance()
        {
            if (null == Current)
            {
                var ga = new GameObject("AudioSourcePool");
                Current = ga.AddComponent<AudioSourceOneShot>();
            }

            return Current;
        }
        
        private readonly Queue<AudioSource> _audioSources = new Queue<AudioSource>();
        private readonly LinkedList<AudioSource> _inuse = new LinkedList<AudioSource>();
        private int _lastCheckFrame = -1;
        
        private void CheckInUse()
        {
            var node = _inuse.First;
            while (node != null)
            {
                var current = node;
                node = node.Next;
 
                if (!current.Value.isPlaying)
                {
                    _audioSources.Enqueue(current.Value);
                    _inuse.Remove(current);
                }
            }
        }
        
        public void PlayAtPoint(SfxSoundLibrary.SoundBundle.ClipConfig clip, Vector3 point)
        {
            AudioSource source;
 
            if (_lastCheckFrame != Time.frameCount)
            {
                _lastCheckFrame = Time.frameCount;
                CheckInUse();
            }
 
            if (_audioSources.Count == 0)
            {
                var ga = new GameObject("Pooled Audio Source");
                ga.transform.parent = transform;
                source = ga.AddComponent<AudioSource>();
            }
            else
                source = _audioSources.Dequeue();
 
            _inuse.AddLast(source);
 
            source.transform.position = point;
            source.clip = clip.clip;
            source.pitch = Random.Range(clip.minPitch, clip.maxPitch);
            source.volume = Random.Range(clip.minVolume, clip.maxVolume);
            
            source.Play();
        }
        
        private void Awake()
        {
            Debug.Assert(null == Current);
            Current = this;
        }

        private  void OnDestroy()
        {
            Debug.Assert(Current == this);
            Current = null;
        }
    }
}