using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collision with obstacle
        if (other.gameObject.CompareTag("Obstacle"))
        {
            // Play player's death sound
            FindObjectOfType<AudioManager>().Play("PlayerDeath");

            // Restart level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
