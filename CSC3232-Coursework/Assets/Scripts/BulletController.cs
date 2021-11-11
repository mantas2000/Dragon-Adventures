using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    private Rigidbody2D _bullet;
    private SpriteRenderer _renderer;
    private bool _isColliding;

    // Start is called before the first frame update
    private void Start()
    {
        // Get bullets travel direction
        var shootDirection = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized * speed;
        
        // Shoot bullet
        _bullet = GetComponent<Rigidbody2D>();
        _bullet.velocity = new Vector2(shootDirection.x, shootDirection.y);
        
        // Flip bullet
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.flipX = shootDirection.x > 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isColliding) return;
        _isColliding = true;
        
        // Collision with player
        if (other.gameObject.CompareTag("Player"))
        {
            // Track how many times player died
            var totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
            PlayerPrefs.SetInt("TotalDeaths", totalDeaths + 1);
            
            // Bullet hits the player, restart the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Destroy bullet after hitting object
        Destroy(gameObject);
    }
    
    private void LateUpdate()
    {
        // Reset collision detection
        _isColliding = false;
    }
}
