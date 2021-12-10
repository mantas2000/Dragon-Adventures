using UnityEngine;

public class MaintainRotation : MonoBehaviour
{
    private void LateUpdate()
    {
        // Maintain initial rotation
        transform.rotation = Quaternion.identity;
    }
}
