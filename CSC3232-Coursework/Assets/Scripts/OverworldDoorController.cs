using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldDoorController : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private Animator animator;
    private bool _previousLevelCompleted;

    private void Start()
    {
        // Make Level 1 always available to play
        PlayerPrefs.SetString("1", "true");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Get previous level index
        var previousLevel = levelIndex - 1;
        
        // Activate doors if previous level is completed
        if (PlayerPrefs.GetString(previousLevel.ToString(), "false") == "true")
        {
            _previousLevelCompleted = true;
        }
        
        animator.SetBool("LevelCompleted", _previousLevelCompleted);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collision with player
        if (other.gameObject.CompareTag("Player") && _previousLevelCompleted)
        {
            // Go to next level
            SceneManager.LoadScene(levelIndex);
        }
    }
}
