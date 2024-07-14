using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class FlipSprite : MonoBehaviour {
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        // Check the horizontal velocity of the Rigidbody2D
        float horizontalVelocity = rb.velocity.x;

        // Flip the sprite based on the horizontal velocity
        if (horizontalVelocity > 0.1f) {
            spriteRenderer.flipX = false;
        }
        else if (horizontalVelocity < -0.1f) {
            spriteRenderer.flipX = true;
        }
    }
}
