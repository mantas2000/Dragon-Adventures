using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GroundEnemySM groundEnemySm;

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Ignore collision if its not with the player
        if (!other.gameObject.CompareTag("Player")) return;
        
        // Player performs Hit Move
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll") && controller.IsGrounded())
        {
            gameObject.SetActive(false);
        }
            
        // Player performs Crouch Move
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Crouch") && !controller.IsGrounded())
        {
            gameObject.SetActive(false);
        }
            
        // Player jumps on enemy and stuns it
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump") && !controller.IsGrounded())
        {
            groundEnemySm.ChangeState(groundEnemySm.StuntState);
        }
        
        // Otherwise, player dies, restart level
        else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
