using UnityEngine;

public class HeartCollisionController : MonoBehaviour
{
    [SerializeField] private float pickupDistance = 1.0f;
    private GameObject player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Collider2D heartCollider = GetComponent<Collider2D>();
            if (playerCollider != null && heartCollider != null)
            {
                Physics2D.IgnoreCollision(heartCollider, playerCollider);
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance <= pickupDistance)
        {
            int hp = PlayerPrefs.GetInt("hp");
            if (PlayerPrefs.GetInt("hp") < 10)
            {
                PlayerPrefs.SetInt("hp", hp + 1);
            }
            Destroy(gameObject);
        }
    }

}
