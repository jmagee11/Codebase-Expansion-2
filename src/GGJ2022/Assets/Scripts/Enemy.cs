using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public int enemyValue = 1;
    public float damage = 0.1f;
    public bool isRed;
    public UnityEvent<int> onKill;
    public UnityEvent onHit;
    public UnityEvent<float, bool> onCrash;

    public int lives = 3;
    public GameObject onDeathEffect;
    public float onDeathEffectScale = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal damage from every color
            // if (other.GetComponent<PlayerWing>().isRed ^ isRed)
            onCrash.Invoke(damage, isRed);
            Delete();
        }
        else if (other.CompareTag("PlayerProjectile"))
        {
            onHit.Invoke();
            if(other.gameObject.GetComponent<Affinity>() && other.gameObject.GetComponent<Affinity>().isRed == isRed)
                return;
            
            
            --lives;
            if (lives <= 0)
            {
                onKill.Invoke(enemyValue);
                EliasManager.instance.introEnemiesDestroyed++;
                EliasManager.instance.MusicLevel();
                Delete();
            }
        }
    }

    private void Delete()
    {
        if (null != onDeathEffect)
        {
            var effect = Instantiate(onDeathEffect, gameObject.transform.parent);
            effect.transform.position = transform.position;
            effect.transform.localEulerAngles = new Vector3(0, 0,Random.Range(0,360));
            effect.transform.localScale = new Vector3(1, 1, 1) * onDeathEffectScale;
        }
        
        Destroy(gameObject);
    }
}

internal class Vector3f
{
    public Vector3f(int i, int i1, int i2)
    {
        throw new NotImplementedException();
    }
}