using UnityEngine;
using Photon.Pun;

public class PlayerMovementAndClimbing : MonoBehaviourPunCallbacks
{
    // Bewegungs- und Sprungvariablen
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    // Klettervariablen
    public float climbSpeed = 5f;
    public LayerMask ladderLayer;

    // Soundeffekte
    public AudioClip jumpSound;
    public AudioClip walkSound;
    private AudioSource audioSource;

    // Referenzen und Zustandsvariablen
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isClimbing = false;
    private float originalGravity;
    private Animator animator;
    private bool facingRight = true;
    PhotonView view;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Stabilität
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        // Überprüfen, ob der Spieler am Boden ist
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (view.IsMine)
        {
            // Kletterbewegung
            if (isClimbing)
            {
                rb.velocity = new Vector2(rb.velocity.x, climbSpeed);
                rb.gravityScale = 0;
            }
            else
            {
                float moveHorizontal = Input.GetAxisRaw("Horizontal");
                rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
                animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

                if (moveHorizontal > 0 && !facingRight)
                {
                    PhotonFlip();
                }
                else if (moveHorizontal < 0 && facingRight)
                {
                    PhotonFlip();
                }

                // Springen
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    PlaySound(jumpSound);
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }

                // Laufen Soundeffekte
                if (moveHorizontal != 0 && isGrounded)
                {
                    if (!audioSource.isPlaying || audioSource.clip != walkSound)
                    {
                        PlaySound(walkSound);
                    }
                }

                rb.gravityScale = originalGravity;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isClimbing = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    void PhotonFlip()
    {
        view.RPC("RPC_Flip", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // Soundeffekt abspielen
    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}