using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    private Rigidbody2D _bullet;
    private GameObject _player;
    private SpriteRenderer _renderer;

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
        // Collision with player
        if (other.gameObject.CompareTag("Player"))
        {
            // Restart level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        // Destroy bullet after hitting object
        Destroy(gameObject);
    }
}