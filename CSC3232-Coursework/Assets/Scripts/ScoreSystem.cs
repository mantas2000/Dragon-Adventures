using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private GameObject emptyPrefab;
    [SerializeField] private GameObject takenPrefab;

    public void DrawGems(int gems, int totalGems)
    {
        // Destroy previous score system
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate updated score system
        for (var i = 0; i < totalGems; i++)
        {
            // Draw all collected gems
            if (i + 1 <= gems)
            {
                var gem = Instantiate(takenPrefab, transform.position, Quaternion.identity);
                gem.transform.SetParent(transform);
            }
            
            // Draw all uncollected gems
            else
            {
                var gem = Instantiate(emptyPrefab, transform.position, Quaternion.identity);
                gem.transform.SetParent(transform);
            }
        }
    }
}
