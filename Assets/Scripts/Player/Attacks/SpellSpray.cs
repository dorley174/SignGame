using UnityEngine;

public class SpellSpray : MonoBehaviour
{
    public float force = 0.13f;

    private Rigidbody2D physic;

    private void Start()
    {
        physic = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        physic.AddForce(new Vector2(force, 0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Collision!");
            if (gameObject.tag == "Poison")
            {
                EffectsManager.Instance.effect.StartPoisoning(other.gameObject);
                Destroy(gameObject);
            }
            else if (gameObject.tag == "Knockback")
            {
                EffectsManager.Instance.effect.ApplyKnockback(transform.position, other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
