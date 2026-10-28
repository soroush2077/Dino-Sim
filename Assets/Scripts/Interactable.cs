using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] float despawnX = -15f;

    bool used;

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm.IsGameOver || gm.IsPaused) return;

        transform.position += Vector3.left * gm.Speed * Time.deltaTime;
        Tick();

        if (transform.position.x < despawnX) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used || !other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerController>();
        if (player == null) return;

        used = true;                 // each object can only trigger once
        OnInteract(player);
    }

    // Children can optionally add extra per-frame behaviour
    protected virtual void Tick() { }

    // Children MUST say what happens when the player touches them
    protected abstract void OnInteract(PlayerController player);
}