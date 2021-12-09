using UnityEngine;

public class MaintainRotation : MonoBehaviour
{
    private Quaternion _rotation;

    // Start is called before the first frame update
    private void Start()
    {
        _rotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // Return to initial rotation
        transform.rotation = _rotation;
    }
}
