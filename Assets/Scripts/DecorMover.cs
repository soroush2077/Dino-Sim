using UnityEngine;

public class DecorMover : MonoBehaviour
{
    public float speedMultiplier = 0.5f;
    public float despawnX = -15f;

    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        transform.Translate(Vector3.left * GameManager.Instance.Speed * speedMultiplier * Time.deltaTime);
        if (transform.position.x < despawnX) Destroy(gameObject);
    }
}