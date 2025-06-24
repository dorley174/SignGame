using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimations : MonoBehaviour
{
    public bool reverse = false;

    [Header("Debug")]
    [SerializeField] bool facingRight;

    PlayerMovement playerMovement;
    Animator animator;
    SpriteRenderer sprite;
    Rigidbody2D rb;

    void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));
        animator.SetBool("IsGrounded", playerMovement.onGround);
        animator.SetBool("IsJumping", !playerMovement.onGround && rb.linearVelocity.y > 0f);
        animator.SetBool("IsFalling", !playerMovement.onGround && rb.linearVelocity.y < 0f);
        if (rb.linearVelocityX != 0) facingRight = IsFacingRight();
        sprite.flipX = facingRight;
    }

    public bool IsFacingRight() {
        return rb.linearVelocityX > 0 != reverse;
    }
}
