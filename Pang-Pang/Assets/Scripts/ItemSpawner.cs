using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public float spawnInterval = 3f;

    private BoxCollider2D spawnArea;

    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        Vector2 randomPos = GetRandomPostionInBox();
        GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        Instantiate(prefab, randomPos, Quaternion.identity);

    }

    Vector2 GetRandomPostionInBox()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }

}
