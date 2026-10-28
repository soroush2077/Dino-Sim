using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public const string HighScoreKey = "HighScore";
    public static GameManager Instance { get; private set; }

    public float Speed { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }
    public int HighScore { get; private set; }
    public int Coins { get; private set; }

    [Header("Gameplay")]
    [SerializeField] float startSpeed = 6f;
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] float maxSpeed = 16f;
    [SerializeField] int scorePerCoin = 50;

    [Header("HUD")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;

    [Header("Game Over Panel")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] GameObject newHighScoreLabel;
    [SerializeField] float panelDelay = 1.2f;

    float score;
    float gameOverTime;
    int finalScore;
    bool isNewBest;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        Speed = startSpeed;
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    void Start()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        UpdateHUD();
    }

    void Update()
    {
        if (IsPaused) return;

        if (IsGameOver)
        {
            if (Time.time - gameOverTime > panelDelay && Keyboard.current.spaceKey.wasPressedThisFrame)
                Restart();
            return;
        }

        Speed = Mathf.Min(Speed + acceleration * Time.deltaTime, maxSpeed);
        score += Speed * Time.deltaTime;
        UpdateHUD();
    }

    public void AddCoin(int amount)
    {
        Coins += amount;
        score += amount * scorePerCoin;
    }

    public void SetPaused(bool paused)
    {
        if (IsGameOver) return;
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        gameOverTime = Time.time;

        finalScore = Mathf.FloorToInt(score);
        isNewBest = finalScore > HighScore;
        if (isNewBest)
        {
            HighScore = finalScore;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
        }

        UpdateHUD();
        Invoke(nameof(ShowGameOverPanel), panelDelay);    // wait so the death animation can play
    }

    void ShowGameOverPanel()
    {
        if (gameOverPanel == null) return;

        gameOverPanel.SetActive(true);
        if (finalScoreText) finalScoreText.text = "SCORE " + finalScore.ToString("D5");
        if (newHighScoreLabel) newHighScoreLabel.SetActive(isNewBest);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void UpdateHUD()
    {
        int current = Mathf.FloorToInt(score);
        int best = Mathf.Max(HighScore, current);

        scoreText.text = current.ToString("D5");
        highScoreText.text = "HI " + best.ToString("D5");
    }
}