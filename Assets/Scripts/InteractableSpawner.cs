using UnityEngine;

public class InteractableSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject standingObstaclePrefab;
    [SerializeField] GameObject flyingObstaclePrefab;
    [SerializeField] GameObject coinPrefab;

    [Header("Relative chances")]
    [SerializeField] float standingWeight = 5f;
    [SerializeField] float flyingWeight = 3f;
    [SerializeField] float coinWeight = 3f;

    [Header("Spacing (in seconds)")]
    [SerializeField] float extraSeconds = 0.5f;          // time on top of one full jump (landing + reaction)
    [SerializeField] float randomExtraSeconds = 0.8f;    // random extra, so it isn't perfectly regular
    [SerializeField] float firstSpawnDelay = 2f;

    [Header("Heights above this object's Y")]
    [SerializeField] float flyingHeight = 1.2f;
    [SerializeField] Vector2 coinHeightRange = new Vector2(0.5f, 2.5f);

    PlayerController player;
    float timer;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        timer = firstSpawnDelay;
    }

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm.IsGameOver || gm.IsPaused) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        SpawnRandom();
        timer = NextDelay();
    }

    // Time until the next spawn: one full jump, plus a safety margin.
    // Objects all move at the same speed, so a fixed time means the gap
    // in world units automatically grows as the game speeds up.
    float NextDelay()
    {
        float airTime = (player != null) ? player.AirTime : 0.6f;
        return airTime + extraSeconds + Random.value * randomExtraSeconds;
    }

    void SpawnRandom()
    {
        float total = standingWeight + flyingWeight + coinWeight;
        float roll = Random.value * total;

        if (roll < standingWeight)
            Spawn(standingObstaclePrefab, 0f);
        else if (roll < standingWeight + flyingWeight)
            Spawn(flyingObstaclePrefab, flyingHeight);
        else
            Spawn(coinPrefab, Random.Range(coinHeightRange.x, coinHeightRange.y));
    }

    void Spawn(GameObject prefab, float height)
    {
        if (prefab == null) return;

        var pos = new Vector3(transform.position.x, transform.position.y + height, 0f);
        Instantiate(prefab, pos, Quaternion.identity);
    }
}