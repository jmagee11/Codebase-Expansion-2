using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using util;

public class GameStateHandler : MonoBehaviour
{
    public int score;
    [SerializeField] private TextMeshProUGUI highscores;
    [SerializeField] private TextMeshProUGUI highscores_lost;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private GameObject gameOverMenu;

    [SerializeField] GameObject healthBar;

    public float PlayerHealth;
    private bool m_InGame;

    public UnityEvent<bool> onIngame;
    public UnityEvent onDeath;

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
        onDeath.Invoke();
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
            onIngame.Invoke(m_InGame);
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