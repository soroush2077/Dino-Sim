using UnityEngine;

public class SpriteFlipbook : MonoBehaviour
{
    [SerializeField] Sprite[] frames;
    [SerializeField] float framesPerSecond = 10f;

    SpriteRenderer sr;
    float timer;
    int index;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (frames.Length > 0)
            index = Random.Range(0, frames.Length);     // so coins don't all spin in sync
    }

    void Update()
    {
        if (frames.Length == 0) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        timer += Time.deltaTime;
        if (timer >= 1f / framesPerSecond)
        {
            timer -= 1f / framesPerSecond;
            index = (index + 1) % frames.Length;
            sr.sprite = frames[index];
        }
    }
}