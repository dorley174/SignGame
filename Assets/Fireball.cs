using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fireball : MonoBehaviour { 
    [SerializeField] ParticleSystem collectEffect;

    [SerializeField] private float _explosionRadius = 3f; // Радиус взрыва
    [SerializeField] private float _explosionForce = 10f; // Сила отталкивания
    [SerializeField] private LayerMask _affectedLayers; // Какие слои будут реагировать
    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        ParticleSystem effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
        effect.Play();

        Explode();
    }
    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _affectedLayers);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Вычисляем направление от центра взрыва
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                // Применяем силу
                rb.AddForce(direction * _explosionForce, ForceMode2D.Impulse);
            }
        }
    }
}