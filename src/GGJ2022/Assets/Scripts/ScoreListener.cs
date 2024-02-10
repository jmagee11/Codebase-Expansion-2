using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreListener : MonoBehaviour
{
    [SerializeField] private GameStateHandler GameStateHandler;

    private TextMeshProUGUI TextMeshProUGUI;

    private void Start()
    {
        TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        TextMeshProUGUI.text = GameStateHandler.GetScore().ToString();
    }
}
