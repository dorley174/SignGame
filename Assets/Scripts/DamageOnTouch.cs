using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public float damage;
    [SerializeField]
    private Player player;

    private void Start()
    {
        
    }
    void OnCollisionStay2D(Collision2D collision) {
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
