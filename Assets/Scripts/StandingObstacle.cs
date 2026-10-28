using UnityEngine;

public class StandingObstacle : Interactable
{
    [SerializeField] int damage = 1;

    protected override void OnInteract(PlayerController player)
    {
        player.TakeDamage(damage);
    }
}