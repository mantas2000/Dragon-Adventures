using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GemCollecting controller;
    private Animator _animator;
    private bool _allGemsCollected;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _allGemsCollected = controller.AllGemsCollected();
        _animator.SetBool("AllGemsCollected", _allGemsCollected);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collision with player
        if (other.gameObject.CompareTag("Player") && _allGemsCollected)
        {
            // Record that player completed a level
            PlayerPrefs.SetString(SceneManager.GetActiveScene().buildIndex.ToString(), "true");
            
            // Go back to overworld
            SceneManager.LoadScene("Overworld");
        }
    }
}
