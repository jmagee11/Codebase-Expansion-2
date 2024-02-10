using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace util
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorRandomTimeOffset : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Animator>().Update(Random.value);
            Destroy(this);
        }
    }
}