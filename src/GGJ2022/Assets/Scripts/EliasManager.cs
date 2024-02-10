using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliasManager : MonoBehaviour
{
    public string activeTheme;
    public static EliasManager instance;
    public int introEnemiesDestroyed;
    public string actionPresetExclusion;
    public GameStateHandler GameStateHandler;
    public float PlayerHealth;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        activeTheme = EliasPlayer.instance.GetActiveTheme();
        PlayerHealth = GameStateHandler.PlayerHealth;
    }

    public void MusicLevel()
    {
        if (activeTheme == "Intro")
        {
            if (introEnemiesDestroyed == 1 && actionPresetExclusion != "IntroLevel2")
            {
                EliasPlayer.instance.RunActionPreset("IntroLevel2");
                actionPresetExclusion = "IntroLevel2";
            }

            if (introEnemiesDestroyed == 2 && actionPresetExclusion != "IntroLevel3")
            {
                EliasPlayer.instance.RunActionPreset("IntroLevel3");
                actionPresetExclusion = "IntroLevel3";
            }

            if (introEnemiesDestroyed == 31 && actionPresetExclusion != "IntroLevel4")
            {
                EliasPlayer.instance.RunActionPreset("IntroLevel4");
                actionPresetExclusion = "IntroLevel4";
            }

            if (introEnemiesDestroyed == 41 && actionPresetExclusion != "IntroLevel5")
            {
                EliasPlayer.instance.RunActionPreset("IntroLevel5");
                actionPresetExclusion = "IntroLevel5";
            }

            if (introEnemiesDestroyed == 5 && actionPresetExclusion != "IntroLevel6")
            {
                EliasPlayer.instance.RunActionPreset("IntroLevel6");
                actionPresetExclusion = "IntroLevel6";
            }

            if (introEnemiesDestroyed == 6 && actionPresetExclusion != "IntroLevel7")
            {
                EliasPlayer.instance.RunActionPreset("IntroLevel7");
                actionPresetExclusion = "IntroLevel7";
            }

            if (introEnemiesDestroyed == 7 && actionPresetExclusion != "ToGameLoopBalanced")
            {
                EliasPlayer.instance.RunActionPreset("ToGameLoopBalanced");
                actionPresetExclusion = "ToGameLoopBalanced";
            }
        }

        if (activeTheme == "GameLoop")
        {
            if (PlayerHealth >= 0.81f && PlayerHealth <= 0.99f && actionPresetExclusion != "ToLevel1")
            {
                actionPresetExclusion = "ToLevel1";
                EliasPlayer.instance.RunActionPreset("ToLevel1");
            }

            if (PlayerHealth >= 0.61f && PlayerHealth <= 0.8f && actionPresetExclusion != "ToLevel2")
            {
                actionPresetExclusion = "ToLevel2";
                EliasPlayer.instance.RunActionPreset("ToLevel2");
            }

            if (PlayerHealth >= 0.4f && PlayerHealth <= 0.6f && actionPresetExclusion != "ToLevel3")
            {
                actionPresetExclusion = "ToLevel3";
                EliasPlayer.instance.RunActionPreset("ToLevel3");
            }

            if (PlayerHealth >= 0.21f && PlayerHealth <= 0.39f && actionPresetExclusion != "ToLevel4")
            {
                actionPresetExclusion = "ToLevel4";
                EliasPlayer.instance.RunActionPreset("ToLevel4");
            }

            if (PlayerHealth >= 0.1f && PlayerHealth <= 0.2f && actionPresetExclusion != "ToLevel5")
            {
                actionPresetExclusion = "ToLevel5";
                EliasPlayer.instance.RunActionPreset("ToLevel5");
            }
        }
    }

    public void InGameStart()
    {
        EliasPlayer.instance.RunActionPreset("IntroStart");
    }

    public void Lost()
    {
        if (activeTheme == "Intro")
        {
            EliasPlayer.instance.RunActionPreset("IntroStart");
        }

        if (activeTheme == "GameLoop")
        {
            EliasPlayer.instance.RunActionPreset("GameOverGameLoop");
        }

        if (activeTheme == "TooBright")
        {
            EliasPlayer.instance.RunActionPreset("GameOverTooBright");
        }
    }
}
