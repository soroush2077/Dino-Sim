using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject firstSelected;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb.escapeKey.wasPressedThisFrame || kb.pKey.wasPressedThisFrame)
            Toggle();
    }

    public void Toggle()
    {
        var gm = GameManager.Instance;
        if (gm.IsGameOver) return;

        bool pause = !gm.IsPaused;
        gm.SetPaused(pause);
        pausePanel.SetActive(pause);

        if (pause)
            EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void Resume()
    {
        if (GameManager.Instance.IsPaused) Toggle();
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }

    public void MainMenu()
    {
        GameManager.Instance.LoadMenu();
    }
}