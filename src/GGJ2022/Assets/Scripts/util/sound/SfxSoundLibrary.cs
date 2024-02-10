using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace util.sound
{
    [CreateAssetMenu(fileName = nameof(SfxSoundLibrary), menuName = "create/SFX/" + nameof(SfxSoundLibrary), order = 0)]
    public class SfxSoundLibrary : ScriptableObject
    {
        [Serializable]
        public class SoundBundle
        {
            [Serializable]
            public enum PlayType
            {
                Sequenziell,
                Random
            }

            [Serializable]
            public class ClipConfig : ISerializationCallbackReceiver
            {
                [SerializeField, HideInInspector]

                private int _serielizedVersion = 0;
                
                public AudioClip clip;
                public float minPitch , maxPitch ;
                public float minVolume, maxVolume ;

                public void OnBeforeSerialize()
                {
                    
                }

                public void OnAfterDeserialize()
                {
                    if (_serielizedVersion == 1)
                        return;

                    minPitch =  maxPitch = 1.0f;
                    minVolume =  maxVolume = 1.0f;

                    //Block future seriealizations overrides
                    _serielizedVersion = 1;
                }
            }

            public string sfxName;
            
            private int _lastPlayed = 0;
            public PlayType selectionMethod = PlayType.Sequenziell;
            public List<ClipConfig> sfx;

            public void PlayNextSound(Vector3 point)
            {
                if (sfx.Count == 0)
                    return;

                ClipConfig config;
                switch (selectionMethod)
                {
                    case PlayType.Sequenziell:
                        config = sfx[_lastPlayed % sfx.Count];
                        break;
                    case PlayType.Random:
                        config = sfx[Random.Range(0, sfx.Count)];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _lastPlayed += 1;

                AudioSourceOneShot.GetInstance().PlayAtPoint(config, point);
            }
        }

        public List<SoundBundle> bundles;
        
        public void PlayNextSound(String sfxName, Vector3 point)
        {
            foreach (var bundle in bundles)
            {
                if (bundle.sfxName == sfxName)
                {
                    bundle.PlayNextSound(point);
                    return;
                }
            }
            
            Debug.Log("Found no sounds for: " + sfxName + " in " + this.name);
        }
    }
}