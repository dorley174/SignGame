using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    [SerializeField] private float attractionForce = 10f;
    [SerializeField] private float detectionRadius = 2f;
    private Rigidbody2D rb;
    private GameObject player;
    private bool isAttracted;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        isAttracted = false;
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        bool isPlayerNearby = distance <= detectionRadius;
        if (isPlayerNearby)
        {
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
            rb.AddForce(directionToPlayer * attractionForce, ForceMode2D.Force);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}