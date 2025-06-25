using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    private GameObject enemyInstance;
    [SerializeField]
    private float respawnTime;
    [SerializeField]
    private TimeUnit respawnTimeUnit = TimeUnit.Seconds;
    private enum TimeUnit
    {
        Milliseconds = 1,
        Seconds = 1000,
        Minutes = 60000
    }
    private float TimeInSeconds => (respawnTime * (int)respawnTimeUnit) / 1000f;
    [SerializeField]
    private Transform spawnPos;
    private Vector3 vectorPos;
    private void OnValidate()
    {
        if (spawnPos == null)
        {
            vectorPos = transform.position;
        }
        else
        {
            vectorPos = spawnPos.position;
        }
    }
    public void SpawnEnemy()
    {
        if (enemyInstance == null)
        {
            enemyInstance = Instantiate(enemyPrefab, vectorPos, enemyPrefab.transform.rotation);
        }
    }
    public void DeleteEnemy()
    {
        if (enemyInstance != null)
        {
            Destroy(enemyInstance); // уничтожается ТОЛЬКО инстанс, а не префаб
            enemyInstance = null;
        }
        StartCoroutine(SpawnAfterSeconds(TimeInSeconds));
    }
    IEnumerator SpawnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SpawnEnemy();
    }
}
