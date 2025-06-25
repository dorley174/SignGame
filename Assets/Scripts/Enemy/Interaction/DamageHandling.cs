using UnityEngine;

public class DamageHandling : MonoBehaviour
{
    public float damage;
    [SerializeField]
    private Player player;
    [SerializeField]
    private EnemyInteractionCharacteristics stats;

    private void Start()
    {
        damage = stats.damage;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (player == null)
        {
            player = collision.gameObject.GetComponent<Player>();
        }
        if (player)
        {
            player.TakeDamage(damage);
        }
    }
}
