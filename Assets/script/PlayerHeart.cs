using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
    [SerializeField] private Player player;

    public void TakeDamage(int damage)
    {
        if (player == null)
        {
            return;
        }

        player.TakeDamage(damage);
    }
}