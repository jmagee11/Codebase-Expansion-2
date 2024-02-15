using scenes;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using util;
using util.sound;

public class GameStateHandler : MonoBehaviour
{
    public int score;
    [SerializeField] private TextMeshProUGUI highscores;
    [SerializeField] private TextMeshProUGUI highscores_lost;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private GameObject gameOverMenu;

    //Get healthbar object
    [SerializeField] GameObject healthBar;

    //get things to replace onDeath unity event
    [SerializeField] GameObject player;
    [SerializeField] GameObject sceneryManager;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject gameOverSound;

    //get remaining things to replace onIngame event
    [SerializeField] GameObject ui;
    [SerializeField] GameObject play;
    [SerializeField] GameObject controls;
    [SerializeField] GameObject highscore;


    public float PlayerHealth;
    private bool m_InGame;

    [SerializeField]
    private PlayerInput inputManage;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth = 0.5f;
        EliasManager.instance.MusicLevel();
        healthBar.GetComponent<FloatAnimator>().OnFloatInput(PlayerHealth);
        StartCoroutine(FirstDisableDelay()); // For whatever reason teh firt menu animation is strange otherwise
    }

    private IEnumerator FirstDisableDelay()
    {
        yield return new WaitForSeconds(0.5f);
        InGame = false;
    }

    public void IncrementScore(int value) => score += value;

    public int GetScore() => score;

    public void ResetScore() => score = 0;

    public void RestartGame()
    {
        PlayerHealth = 0.5f;
        EliasManager.instance.MusicLevel();
        healthBar.GetComponent<FloatAnimator>().OnFloatInput(PlayerHealth);
        gameOverMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ResetScore();
        InGame = true;
    }

    public void BackToMenuAfterDeath() => m_InGame = false;
    

    public void Lost()
    {
        //replace onDeath here
        player.SetActive(false);
        sceneryManager.GetComponent<LevelSceneryManager>().EnableEnemySpawning(false);
        UpdateHighScores();
        gameOver.SetActive(true);
        gameOverSound.GetComponent<SfxSoundLibraryUser>().PlaySound();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        EliasManager.instance.Lost();
        EliasManager.instance.introEnemiesDestroyed = 0;
    }

    public void DamagePlayer(float damage, bool isRed)
    {
        if (!InGame)
            return;

        PlayerHealth += isRed ? damage : -damage;

        EliasManager.instance.MusicLevel();
        healthBar.GetComponent<FloatAnimator>().OnFloatInput(PlayerHealth);
        if (PlayerHealth >= 1f || PlayerHealth <= 0f)
            Lost();
    }

    public bool InGame
    {
        get => m_InGame;
        private set
        {
            m_InGame = value;
            inputManage.currentActionMap = inputManage.actions.FindActionMap(m_InGame ? "InGame" : "InMenu", true);
            //replace onIngame here
            
            sceneryManager.GetComponent<LevelSceneryManager>().EnableEnemySpawning(m_InGame);
            ui.GetComponents<BooleanAnimator>()[1].OnBooleanInput(m_InGame);
            player.SetActive(m_InGame);
            player.GetComponent<OnEnableAnimatorReset>().ResetAnimator();
        }
    }

    public void UpdateHighScores()
    {
        highscores_lost.text = highscores.text = "Loading ...";
        StartCoroutine(HighScore.RequestTop10(res =>
        {
            highscores_lost.text = highscores.text = res.could_load
                ? string.Join("\n", res.highscores.Select((hs) => hs.name + " - " + hs.value))
                : "Error loading high scores.";
        }));
    }

    public void PostScore()
    {
        if (playerName.text.Trim() == "") return;
        StartCoroutine(HighScore.UploadHighScore(new HighScore.Entry
        {
            name = playerName.text,
            value = score
        }));
    }
}