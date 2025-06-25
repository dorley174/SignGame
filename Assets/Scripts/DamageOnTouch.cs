using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public float damage = 3f;

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) collision.gameObject.GetComponent<Player>().TakeDamage(damage);
    }
}
