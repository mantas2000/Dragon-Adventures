using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GroundEnemySM groundEnemySm;
    [SerializeField] private GemCollecting gemCollecting;
    private bool _isColliding;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_isColliding) return;
        _isColliding = true;
        
        // Ignore collision if its not with the player
        if (!other.gameObject.CompareTag("Player")) return;
        
        // Player performs Dash Move
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll") && controller.IsGrounded())
        {
            // Play enemy's death sound
            FindObjectOfType<AudioManager>().Play("Hit");
            
            // Track how many times player killed enemies
            var totalKills = PlayerPrefs.GetInt("TotalKills", 0);
            PlayerPrefs.SetInt("TotalKills", totalKills + 1);
            
            gameObject.SetActive(false);
            gemCollecting.EnemyDefeated();
        }
            
        // Player performs Duck Move
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Crouch") && !controller.IsGrounded())
        {
            // Play enemy's death sound
            FindObjectOfType<AudioManager>().Play("Hit");
            
            // Track how many times player killed enemies
            var totalKills = PlayerPrefs.GetInt("TotalKills", 0);
            PlayerPrefs.SetInt("TotalKills", totalKills + 1);
            
            gameObject.SetActive(false);
            gemCollecting.EnemyDefeated();
        }
            
        // Player jumps on enemy and stuns it
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump") && !controller.IsGrounded())
        {
            // Play enemy's stun sound
            FindObjectOfType<AudioManager>().Play("EnemyStun");
            
            groundEnemySm.ChangeState(groundEnemySm.StuntState);
        }
        
        // Otherwise, player dies, restart level
        else
        {
            // Play player's death sound
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
            
            // Track how many times player died
            var totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
            PlayerPrefs.SetInt("TotalDeaths", totalDeaths + 1);
            
            // Reload level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void LateUpdate()
    {
        // Reset collision detection
        _isColliding = false;
    }
}
