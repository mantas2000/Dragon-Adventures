using UnityEngine;

public class DestructibleController : MonoBehaviour
{
    public CharacterController2D controller;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Collision with player
        if (other.gameObject.CompareTag("Player") && controller.AirAttack())
        {
            gameObject.SetActive(false);
        }
    }
}
