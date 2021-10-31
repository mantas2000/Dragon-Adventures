using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collision with player
        if (other.gameObject.CompareTag("Obstacle"))
        {
            // Restart level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
