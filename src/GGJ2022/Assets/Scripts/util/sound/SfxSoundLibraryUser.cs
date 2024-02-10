using System;
using UnityEngine;

namespace util.sound
{
    public class SfxSoundLibraryUser : MonoBehaviour
    {
        public SfxSoundLibrary library;
        public string sound;

        public void SetSfxName(string name) =>
            sound = name;
        
        public void PlaySound() =>
            library.PlayNextSound(sound, transform.position);
    }
}