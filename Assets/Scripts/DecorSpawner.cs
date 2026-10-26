using UnityEngine;

public class DecorSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] Vector2 interval = new(1f, 3f);              // seconds between spawns
    [SerializeField] Vector2 yRange = new(-1f, 2f);               // height range to place them
    [SerializeField] Vector2 scaleRange = new(0.8f, 1.3f);
    [SerializeField, Range(0.1f, 1f)] float speedMultiplier = 0.5f;
    [SerializeField] float despawnX = -15f;
    [SerializeField] Color tint = new(0.85f, 0.9f, 1f, 1f);       // slight bluish haze = looks farther away

    float timer;

    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        var prefab = prefabs[Random.Range(0, prefabs.Length)];
        var pos = new Vector3(transform.position.x, Random.Range(yRange.x, yRange.y), 0f);
        var obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.transform.localScale *= Random.Range(scaleRange.x, scaleRange.y);
        obj.GetComponent<SpriteRenderer>().color = tint;

        var mover = obj.AddComponent<DecorMover>();
        mover.speedMultiplier = speedMultiplier;
        mover.despawnX = despawnX;

        timer = Random.Range(interval.x, interval.y);
    }
}

