using UnityEngine;

public class DestructibleWallController : MonoBehaviour
{
    [SerializeField] private Animator controller;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Collision with player
        if (other.gameObject.CompareTag("Player") && controller.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
        {
            gameObject.SetActive(false);
        }
    }
}
