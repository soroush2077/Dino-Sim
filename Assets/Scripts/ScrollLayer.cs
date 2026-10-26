using UnityEngine;

public class ScrollLayer : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float speedMultiplier = 1f;   // ground = 1, far hills = 0.2
    [SerializeField] float loopDistance = 1f;                      // width of ONE repeating tile, in world units
    [SerializeField] float direction = -1f;                        // -1 = scenery moves left

    Vector3 startPos;

    void Start() => startPos = transform.position;

    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        float move = GameManager.Instance.Speed * speedMultiplier * Time.deltaTime * direction;
        transform.position += new Vector3(move, 0f, 0f);

        if (Mathf.Abs(transform.position.x - startPos.x) >= loopDistance)
            transform.position = startPos;
    }
}