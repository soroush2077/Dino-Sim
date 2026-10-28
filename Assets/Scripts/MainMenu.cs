using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreText;

    void Start()
    {
        Time.timeScale = 1f;
        highScoreText.text = "HIGH SCORE " + PlayerPrefs.GetInt(GameManager.HighScoreKey, 0).ToString("D5");
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}