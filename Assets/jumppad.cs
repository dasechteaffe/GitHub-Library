using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f; // Kraft des Sprungs

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Anwenden einer Aufwärtskraft auf den Spieler
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}