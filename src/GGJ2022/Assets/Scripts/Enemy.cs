using System;
using UnityEngine;
using UnityEngine.Events;
using util.sound;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public int enemyValue = 1;
    public float damage = 0.1f;
    public bool isRed;

    //libray to use in replacing sound events
    [SerializeField] SfxSoundLibrary library;
    [SerializeField] GameObject manager;

    public int lives = 3;
    public GameObject onDeathEffect;
    public float onDeathEffectScale = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal damage from every color
            // if (other.GetComponent<PlayerWing>().isRed ^ isRed)
            //replace onCrash here
            GameObject.Find("GameState").GetComponent<GameStateHandler>().DamagePlayer(damage, isRed);
            if (isRed)
            {
                library.PlayNextSound("enemy/red", transform.position);
            }
            else
            {
                library.PlayNextSound("enemy/white", transform.position);
            }


            Delete();
        }
        else if (other.CompareTag("PlayerProjectile"))
        {
            //replace onHit here
            library.PlayNextSound("enemy/hit", transform.position);
            if (other.gameObject.GetComponent<Affinity>() && other.gameObject.GetComponent<Affinity>().isRed == isRed)
                return;
            
            
            --lives;
            if (lives <= 0)
            {
                //replace onKill here
                GameObject.Find("GameState").GetComponent<GameStateHandler>().IncrementScore(enemyValue);
                if (isRed)
                {
                    library.PlayNextSound("enemy/red", transform.position);
                }
                else
                {
                    library.PlayNextSound("enemy/white", transform.position);
                }
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