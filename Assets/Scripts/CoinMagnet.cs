using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    [SerializeField] private float attractionForce = 10f;
    private Rigidbody2D rb;
    private Transform playerTransform;
    private bool isAttracted;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isAttracted = false;
    }

    void FixedUpdate()
    {
        if (isAttracted && playerTransform != null)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            rb.AddForce(directionToPlayer * attractionForce, ForceMode2D.Force);
        }
    }

    public void OnTriggerEnterChild(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isAttracted = true;
        }
    }

    public void OnTriggerExitChild(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isAttracted = false;
            playerTransform = null;
        }
    }
}