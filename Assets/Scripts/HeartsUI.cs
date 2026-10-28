using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    PlayerController player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (player == null) return;

        for (int i = 0; i < hearts.Length; i++)
            hearts[i].sprite = (i < player.Lives) ? fullHeart : emptyHeart;
    }
}