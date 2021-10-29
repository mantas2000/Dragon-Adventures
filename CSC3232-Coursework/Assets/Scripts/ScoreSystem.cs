using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private GameObject emptyPrefab;
    [SerializeField] private GameObject takenPrefab;

    public void DrawGems(int gems, int totalGems)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (var i = 0; i < totalGems; i++)
        {
            if (i + 1 <= gems)
            {
                var gem = Instantiate(takenPrefab, transform.position, Quaternion.identity);
                gem.transform.SetParent(transform);
            }
            else
            {
                var gem = Instantiate(emptyPrefab, transform.position, Quaternion.identity);
                gem.transform.SetParent(transform);
            }
        }
    }
}
