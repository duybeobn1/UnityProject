using System.Collections;
using UnityEngine;

public class SpiderGenerator : MonoBehaviour
{
    public GameObject spiderPrefab;
    public int maxNumberOfSpiders = 20;
    public float spawnRadius = 10f;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    public Transform mapPreviewTransform; // Reference to the MapPreview object's transform

    private int currentNumberOfSpiders = 0;

    private void Start()
    {
        StartCoroutine(SpawnSpiders());
    }

    private IEnumerator SpawnSpiders()
    {
        while (currentNumberOfSpiders < maxNumberOfSpiders)
        {
            // Generate a random position within the specified spawn radius
            Vector3 randomPosition = Random.insideUnitCircle * spawnRadius * 20f;
            randomPosition += transform.position; // Offset by parent's position
            randomPosition.y = 10f; // Ensure spiders spawn on the ground

            // Instantiate the spider prefab at the random position under MapPreview
            GameObject newSpider = Instantiate(spiderPrefab, randomPosition, Quaternion.identity, mapPreviewTransform);
            currentNumberOfSpiders++;

            // Wait for a random delay before spawning the next spider
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

    public void SpiderDestroyed(GameObject destroyedSpider)
    {
        currentNumberOfSpiders--;
        if (currentNumberOfSpiders < maxNumberOfSpiders)
        {
            StartCoroutine(SpawnSpiderAfterDelay());
        }
    }

    private IEnumerator SpawnSpiderAfterDelay()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        // Generate a random position within the specified spawn radius
        Vector3 randomPosition = Random.insideUnitCircle * spawnRadius * 20f;
        randomPosition += transform.position; // Offset by parent's position
        randomPosition.y = 10f; // Ensure spiders spawn on the ground

        // Instantiate the spider prefab at the random position under MapPreview
        GameObject newSpider = Instantiate(spiderPrefab, randomPosition, Quaternion.identity, mapPreviewTransform);
        currentNumberOfSpiders++;
    }
}
