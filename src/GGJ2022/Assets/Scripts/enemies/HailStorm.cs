using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace enemies
{
    public class HailStorm : MonoBehaviour
    {
        public float duration = 5;
        public float waitMin = 0.05f, waitMax = 0.5f;
        public float width = 5;
        public float angleOffset = 30;
        public UnityEvent triggerSound;
        public List<GameObject> projectiles;

        public void Start()
        {
            StartCoroutine(HailStormSpawn());
        }

        private IEnumerator  HailStormSpawn()
        {
            // When we are an enemy, wire our shots up.
            var parentEnemy = GetComponent<Enemy>();
            
            var start = Time.time;
            while (Time.time - start <= duration)
            {
                var p = Instantiate(projectiles[Random.Range(0, projectiles.Count)], transform.parent);
                p.transform.position = transform.TransformPoint(new Vector3(Random.Range(-width / 2, width / 2), 0, 0));
                p.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-angleOffset, angleOffset));

                var shotEnemy = p.GetComponent<Enemy>();
                if (null != shotEnemy && null != parentEnemy)
                {
                    shotEnemy.onCrash.AddListener(parentEnemy.onCrash.Invoke);
                    shotEnemy.onKill.AddListener(parentEnemy.onKill.Invoke);
                }

                yield return new WaitForSeconds(Random.Range(waitMin, waitMax));
                triggerSound.Invoke();
            }
            
            yield return new WaitForSeconds(10);
            
            Destroy(gameObject);
        }
    }
}