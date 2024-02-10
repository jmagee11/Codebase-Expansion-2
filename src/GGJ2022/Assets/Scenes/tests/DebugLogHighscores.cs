using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using util;

public class DebugLogHighscores : MonoBehaviour
{
    public Text highscores;
    
    public Text player_name;
    
    public Slider score;

    // Start is called before the first frame update
    void Start() =>
        UpdateHighScores();

    public void UpdateHighScores()
    {
        highscores.text = "Loading ...";
        StartCoroutine(HighScore.RequestTop10(res =>
        {
            highscores.text = res.could_load
                ? string.Join("\n", res.highscores.Select((hs) => hs.name + " - " + hs.value))
                : "Error loading high scores.";
        }));
    }

    public void PostScore()
    {
        StartCoroutine(HighScore.UploadHighScore(new HighScore.Entry
        {
            name = player_name.text,
            value = (int)score.value
        }));
    }
}