using System.Collections;
using UnityEngine;

public class GemCollecting : MonoBehaviour
{
    [SerializeField] private ScoreSystem scoreSystem;
    private bool _isColliding;
    private int _gemsCollected;
    private int _totalGems;
    
    private void Start()
    {
        _totalGems = GameObject.FindGameObjectsWithTag("Gem").Length + GameObject.FindGameObjectsWithTag("Enemy").Length;
        scoreSystem.DrawGems(_gemsCollected, _totalGems);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isColliding) return;
        _isColliding = true;
        
        // Collision with gems
        if (other.gameObject.CompareTag("Gem"))
        {
            // Play sound
            FindObjectOfType<AudioManager>().Play("GemCollected");
            
            other.gameObject.SetActive(false);
            _gemsCollected += 1;
            
            // Update scoreboard
            scoreSystem.DrawGems(_gemsCollected, _totalGems);
        }
        
        // Reset collision trigger
        StartCoroutine(CollisionReset());
    }

    // Reset collision trigger
    private IEnumerator CollisionReset()
    {
        yield return new WaitForEndOfFrame();
        _isColliding = false;
    }

    // Check if player collected all available gems
    public bool AllGemsCollected()
    {
        return _gemsCollected >= _totalGems;
    }

    // Reward player after beating enemies
    public void EnemyDefeated()
    {
        _gemsCollected += 1;
        
        // Update scoreboard
        scoreSystem.DrawGems(_gemsCollected, _totalGems);
    }
}
