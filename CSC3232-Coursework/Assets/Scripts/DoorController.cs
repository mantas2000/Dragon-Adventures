using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private CharacterController2D controller2D;
    private Animator _animator;
    private bool _allGemsCollected;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _allGemsCollected = controller2D.AllGemsCollected();
        _animator.SetBool("AllGemsCollected", _allGemsCollected);
    }
}
