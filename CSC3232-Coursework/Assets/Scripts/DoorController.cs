using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GemCollecting controller;
    private Animator _animator;
    private bool _allGemsCollected;
    private bool _levelCompleted;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Enable doors if all gems are collected
        _allGemsCollected = controller.AllGemsCollected();

        // If all gems collected, play level completion sound
        if (_allGemsCollected && !_levelCompleted)
        {
            _levelCompleted = true;
            FindObjectOfType<AudioManager>().Play("LevelCompleted");
        }
        _animator.SetBool("AllGemsCollected", _allGemsCollected);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check whether the player is colliding with the doors and collected all gems
        if (other.gameObject.CompareTag("Player") && _allGemsCollected)
        {
            // Record that player completed a level
            PlayerPrefs.SetString(SceneManager.GetActiveScene().buildIndex.ToString(), "true");
            
            // Go back to overworld
            SceneManager.LoadScene("Overworld");
        }
    }
}
