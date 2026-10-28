using UnityEngine;

public class Coin : Interactable
{
    [SerializeField] int coinValue = 1;

    protected override void OnInteract(PlayerController player)
    {
        GameManager.Instance.AddCoin(coinValue);
        Destroy(gameObject);
    }
}