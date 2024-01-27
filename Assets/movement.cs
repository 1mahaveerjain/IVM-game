using System.Collections;

/*public class SimpleBallMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Basic movement
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // Jumping
      
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground check
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Ground check
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
*/

using UnityEngine;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 7f;
    public float wallJumpForce = 10f;
    public float dashForce = 20f;
    public float dashDuration = 0.2f;

    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Basic movement
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump(jumpForce);
            }
            else if (canDoubleJump)
            {
                Jump(doubleJumpForce);
                canDoubleJump = false;
            }
        }

        // Wall Jumping
        if (Input.GetButtonDown("Jump") && !isGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 0.1f);
            if (hit.collider != null)
            {
                WallJump();
            }
        }

        // Dashing
        if (Input.GetButtonDown("Dash") && !isDashing)
        {
            Dash();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground check
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Ground check
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Jump(float jumpForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void WallJump()
    {
        rb.velocity = new Vector2(0f, 0f);
        rb.AddForce(new Vector2(-transform.localScale.x, 1f) * wallJumpForce, ForceMode2D.Impulse);
    }

    void Dash()
    {
        StartCoroutine(DashCooldown());
        rb.velocity = new Vector2(rb.velocity.x + (transform.localScale.x * dashForce), rb.velocity.y);
    }

    IEnumerator DashCooldown()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
}