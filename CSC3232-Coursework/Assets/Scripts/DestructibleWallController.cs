using Cinemachine;
using UnityEngine;

public class DestructibleWallController : MonoBehaviour
{
    [SerializeField] private Animator controller;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Collision with player
        if (other.gameObject.CompareTag("Player") && controller.GetCurrentAnimatorStateInfo(0).IsName("Player_Roll"))
        {
            // Shake camera
            impulseSource.GenerateImpulse();
            
            // Play wall break sound
            FindObjectOfType<AudioManager>().Play("Hit");
            
            gameObject.SetActive(false);
        }
    }
}
