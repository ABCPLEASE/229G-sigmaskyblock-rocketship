using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public string meteoriteTag = "Meteorite";

    public float enemySpawnInterval = 5f;
    public float meteoriteSpawnInterval = 2f;

    public Transform[] enemySpawnPoints;
    public Transform[] meteoriteSpawnPoints;

    public float meteoriteMinX = -8f;
    public float meteoriteMaxX = 8f;
    public float meteoriteSpawnY = 6f;

    private float enemyTimer = 0f;
    private float meteoriteTimer = 0f;

    void Update()
    {
        enemyTimer += Time.deltaTime;
        meteoriteTimer += Time.deltaTime;

        if (enemyTimer >= enemySpawnInterval)
        {
            SpawnEnemy();
            enemyTimer = 0f;
        }

        if (meteoriteTimer >= meteoriteSpawnInterval)
        {
            SpawnMeteorite();
            meteoriteTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemySpawnPoints.Length > 0)
        {
            Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
            ObjectPooler.Instance.SpawnFromPool(enemyTag, spawnPoint.position, Quaternion.identity);
        }
    }

    void SpawnMeteorite()
    {
        Transform spawnPoint = meteoriteSpawnPoints[Random.Range(0, meteoriteSpawnPoints.Length)];
        ObjectPooler.Instance.SpawnFromPool(meteoriteTag, spawnPoint.position, Quaternion.identity);

    }
}
