using UnityEngine;

public class FlyingObstacle : Interactable
{
    [SerializeField] int damage = 1;
    [SerializeField] float bobAmount = 0.3f;
    [SerializeField] float bobSpeed = 3f;

    float startY;
    float phase;

    void Start()
    {
        startY = transform.position.y;
        phase = Random.value * 6.28f;        // so several birds don't bob in sync
    }

    protected override void Tick()
    {
        float y = startY + Mathf.Sin(Time.time * bobSpeed + phase) * bobAmount;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    protected override void OnInteract(PlayerController player)
    {
        player.TakeDamage(damage);
    }
}