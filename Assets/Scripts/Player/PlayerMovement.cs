using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    public float horizontalVelocity = 3f;
    public float jumpVelocity = 5f;
    public float groundDetectionOffset;
    public LayerMask playerLayerMask;

    [Tooltip("Debug")]
    [SerializeField] bool onGround = false;

    Rigidbody2D rb;
    CapsuleCollider2D col;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    void Update() {
        CheckOnGround();
        HandleJump();
        HandleMovement();
    }

    void CheckOnGround() {
        onGround = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.down, groundDetectionOffset, ~playerLayerMask);
    }

    void HandleJump() {
        if (onGround && Input.GetButtonDown("Jump")) rb.linearVelocityY = jumpVelocity;
    }

    void HandleMovement() {
        rb.linearVelocityX = Input.GetAxis("Horizontal") * horizontalVelocity;
    }
}
