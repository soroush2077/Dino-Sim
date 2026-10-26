using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float Speed { get; private set; }
    public bool IsGameOver { get; private set; }
    public int HighScore { get; private set; }

    [SerializeField] float startSpeed = 5f;
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;

    float score;
    int stars;

    void Awake()
    {
        Instance = this;
        Speed = startSpeed;
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Start()
    {
        UpdateTexts();
    }

    void Update()
    {
        if (IsGameOver)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        Speed += acceleration * Time.deltaTime;
        score += Speed * Time.deltaTime;
        UpdateTexts();
    }

    public void AddStars(int amount)
    {
        stars += amount;
        score += amount * 50f;      // bonus points; remove if you want stars to be separate
    }

    public void GameOver()
    {
        if (IsGameOver) return;     // ignore repeat calls
        IsGameOver = true;

        int final = Mathf.FloorToInt(score);
        if (final > HighScore)
        {
            HighScore = final;
            PlayerPrefs.SetInt("HighScore", HighScore);
            PlayerPrefs.Save();
        }
        UpdateTexts();
    }

    void UpdateTexts()
    {
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
        highScoreText.text = "HI " + HighScore.ToString("D5");
    }
}